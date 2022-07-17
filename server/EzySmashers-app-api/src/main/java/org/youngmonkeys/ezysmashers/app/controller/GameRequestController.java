package org.youngmonkeys.ezysmashers.app.controller;

import com.tvd12.ezyfox.bean.annotation.EzyAutoBind;
import com.tvd12.ezyfox.core.annotation.EzyDoHandle;
import com.tvd12.ezyfox.core.annotation.EzyRequestController;
import com.tvd12.ezyfox.io.EzyLists;
import com.tvd12.ezyfox.util.EzyLoggable;
import com.tvd12.ezyfoxserver.entity.EzyUser;
import com.tvd12.ezyfoxserver.support.factory.EzyResponseFactory;
import com.tvd12.gamebox.constant.RoomStatus;
import com.tvd12.gamebox.entity.MMOPlayer;
import com.tvd12.gamebox.entity.MMORoom;
import com.tvd12.gamebox.entity.Player;
import org.youngmonkeys.ezysmashers.app.constant.Commands;
import org.youngmonkeys.ezysmashers.app.exception.JoinNotWaitingRoomException;
import org.youngmonkeys.ezysmashers.app.game.shared.PlayerHitData;
import org.youngmonkeys.ezysmashers.app.game.shared.PlayerInputData;
import org.youngmonkeys.ezysmashers.app.game.shared.PlayerSpawnData;
import org.youngmonkeys.ezysmashers.app.request.JoinMMORoomRequest;
import org.youngmonkeys.ezysmashers.app.request.PlayerHitDataRequest;
import org.youngmonkeys.ezysmashers.app.request.PlayerInputDataRequest;
import org.youngmonkeys.ezysmashers.app.service.GamePlayService;
import org.youngmonkeys.ezysmashers.app.service.LobbyService;
import org.youngmonkeys.ezysmashers.app.service.RoomService;
import lombok.Setter;

import java.util.ArrayList;
import java.util.List;

@Setter
@EzyRequestController
public class GameRequestController extends EzyLoggable {

    @EzyAutoBind
    private LobbyService lobbyService;

    @EzyAutoBind
    private RoomService roomService;

    @EzyAutoBind
    private GamePlayService gamePlayService;

    @EzyAutoBind
    private EzyResponseFactory responseFactory;

    @EzyDoHandle(Commands.JOIN_LOBBY)
    public void joinLobby(EzyUser user) {
        logger.info("user {} join lobby room", user);

        lobbyService.addNewPlayer(user.getName());
        long lobbyRoomId = lobbyService.getRoomId();

        responseFactory.newObjectResponse()
            .command(Commands.JOIN_LOBBY)
            .param("lobbyRoomId", lobbyRoomId)
            .user(user)
            .execute();
    }

    @EzyDoHandle(Commands.CREATE_MMO_ROOM)
    public void createMMORoom(EzyUser user) {
        logger.info("user {} create an MMO room", user);
        MMORoom room = roomService.newMMORoom(user);

        responseFactory.newObjectResponse()
            .command(Commands.CREATE_MMO_ROOM)
            .param("roomId", room.getId())
            .user(user)
            .execute();
    }

    @EzyDoHandle(Commands.GET_MMO_ROOM_ID_LIST)
    public void getMMORoomIdList(EzyUser user) {
        logger.info("user {} get MMO room list", user);
        List<Long> mmoRoomIdList = roomService.getMMORoomIdList();
        responseFactory.newArrayResponse()
            .command(Commands.GET_MMO_ROOM_ID_LIST)
            .param(mmoRoomIdList)
            .user(user)
            .execute();
    }

    @EzyDoHandle(Commands.GET_MMO_ROOM_PLAYERS)
    public void getMMORoomPlayers(EzyUser user) {
        logger.info("user {} getMMORoomPlayers", user);
        MMORoom currentRoom = (MMORoom) roomService.getCurrentRoom(user.getName());
        List<String> players = roomService.getRoomPlayerNames(currentRoom);
        Player master = roomService.getMaster(currentRoom);

        responseFactory.newObjectResponse()
            .command(Commands.GET_MMO_ROOM_PLAYERS)
            .param("players", players)
            .param("master", master.getName())
            .user(user)
            .execute();
    }

    @EzyDoHandle(Commands.JOIN_MMO_ROOM)
    public void joinMMORoom(EzyUser user, JoinMMORoomRequest request) {
        logger.info("user {} join room {}", user.getName(), request.getRoomId());
        long roomId = request.getRoomId();
        MMORoom room = roomService.playerJoinMMORoom(user.getName(), roomId);
        if (room.getStatus() != RoomStatus.WAITING) {
            throw new JoinNotWaitingRoomException(user.getName(), room);
        }
        List<String> playerNames = roomService.getRoomPlayerNames(room);

        responseFactory.newObjectResponse()
            .command(Commands.JOIN_MMO_ROOM)
            .param("roomId", roomId)
            .user(user)
            .execute();

        responseFactory.newObjectResponse()
            .command(Commands.ANOTHER_JOIN_MMO_ROOM)
            .param("playerName", user.getName())
            .usernames(EzyLists.filter(playerNames, it -> !it.equals(user.getName())))
            .execute();
    }

    @EzyDoHandle(Commands.START_GAME)
    public void startGame(EzyUser user) {
        logger.info("user {} start game", user);
        MMORoom currentRoom = (MMORoom) roomService.getCurrentRoom(user.getName());
        currentRoom.setStatus(RoomStatus.PLAYING);
        List<String> playerNames = roomService.getRoomPlayerNames(currentRoom);
        gamePlayService.resetPlayersPositionHistory(playerNames);

        List<PlayerSpawnData> data = gamePlayService.spawnPlayers(playerNames);

        responseFactory.newArrayResponse()
            .command(Commands.START_GAME)
            .data(data)
            .usernames(playerNames)
            .execute();
    }

    @EzyDoHandle(Commands.PLAYER_INPUT_DATA)
    public void handlePlayerInputData(EzyUser user, PlayerInputDataRequest request) {
        logger.info("user {} send input data {}", user.getName(), request);
        gamePlayService.handlePlayerInputData(
            user.getName(),
            new PlayerInputData(request.getK(), request.getT()),
            request.getR()
        );
    }

    @EzyDoHandle(Commands.PLAYER_HIT)
    public void handlePlayerHit(EzyUser user, PlayerHitDataRequest request) {
        logger.info("user {} send hit command {}", user.getName(), request);
        PlayerHitData playerHitData = new PlayerHitData(
            request.getP(),
            request.getM(),
            request.getO(),
            request.getV()
        );

        boolean isValidHit = gamePlayService.authorizeHit(
            user.getName(),
            playerHitData
        );

        MMORoom currentRoom = (MMORoom) roomService.getCurrentRoom(user.getName());
        List<String> playerNames = roomService.getRoomPlayerNames(currentRoom);

        if (isValidHit) {
            responseFactory.newObjectResponse()
                .command(Commands.PLAYER_BEING_ATTACKED)
                .param("a", user.getName())
                .param("t", playerHitData.getMyClientTick())
                .param("p", playerHitData.getAttackPosition())
                .param("b", playerHitData.getVictimName())
                .usernames(playerNames)
                .execute();

            roomService.removePlayerFromGameRoom(playerHitData.getVictimName(), currentRoom);
        } else {
            logger.warn("Player {} send invalid hit ", user.getName());
        }
    }

    @EzyDoHandle(Commands.PLAYER_ATTACK_DATA)
    public void handlePlayerAttackData(EzyUser user) {
        logger.info("user {} send attack command", user.getName());

        MMOPlayer player = roomService.getPlayer(user.getName());
        List<String> nearbyPlayerNames = new ArrayList<>();
        player.getNearbyPlayerNames(nearbyPlayerNames);

        responseFactory.newObjectResponse()
            .command(Commands.PLAYER_ATTACK_DATA)
            .param("a", user.getName())
            .usernames(nearbyPlayerNames)
            .execute();
    }
}
