package org.youngmonkeys.ezysmashers.app.service;

import com.tvd12.ezyfox.bean.annotation.EzyAutoBind;
import com.tvd12.ezyfox.bean.annotation.EzySingleton;
import com.tvd12.ezyfox.util.EzyLoggable;
import com.tvd12.ezyfoxserver.entity.EzyUser;
import com.tvd12.gamebox.constant.RoomStatus;
import com.tvd12.gamebox.entity.MMOPlayer;
import com.tvd12.gamebox.entity.MMORoom;
import com.tvd12.gamebox.entity.Player;
import com.tvd12.gamebox.manager.PlayerManager;
import com.tvd12.gamebox.math.Vec3;
import lombok.Setter;
import org.youngmonkeys.ezysmashers.app.constant.GameConstants;
import org.youngmonkeys.ezysmashers.app.exception.InvalidPlayerHitException;
import org.youngmonkeys.ezysmashers.app.model.*;
import org.youngmonkeys.ezysmashers.app.utils.PlayerUtils;

import java.util.*;
import java.util.concurrent.ThreadLocalRandom;
import java.util.stream.Collectors;

import static org.youngmonkeys.ezysmashers.app.constant.GameConstants.*;

@Setter
@EzySingleton
public class GamePlayService extends EzyLoggable {

    @EzyAutoBind
    RoomService roomService;

    @EzyAutoBind
    private PlayerManager<Player> globalPlayerManager;

    private Map<String, SortedMap<Integer, Vec3>> positionHistoryByPlayerName = new HashMap<>();

    public void handlePlayerInput(
        String playerName,
        PlayerInputModel model
    ) {
        MMOPlayer player = roomService.getPlayer(playerName);
        synchronized (player) {
            Vec3 currentPosition = player.getPosition();
            Vec3 currentRotation = player.getRotation();
            Vec3 nextPosition = PlayerUtils.getNextPosition(model, currentPosition);
            Vec3 nextRotation = new Vec3(model.getRotation());
            boolean playerOutsideMap =
                nextPosition.getX() <= MAP_BORDER_OFFSET
                    || nextPosition.getX() >= (MAP_MAX_X - MAP_BORDER_OFFSET)
                    || nextPosition.getZ() <= MAP_BORDER_OFFSET
                    || nextPosition.getZ() >= (MAP_MAX_Z - MAP_BORDER_OFFSET);
            if (playerOutsideMap) {
                nextPosition = currentPosition;
                nextRotation = currentRotation;
            }
            logger.info("next position = {}", nextPosition);
            roomService.setPlayerPosition(player, nextPosition);
            player.setRotation(nextRotation);
            player.setClientTimeTick(model.getTime());

            SortedMap<Integer, Vec3> playerPositionHistory = positionHistoryByPlayerName.get(
                playerName);
            playerPositionHistory.put(model.getTime(), nextPosition);
            if (playerPositionHistory.size() > GameConstants.MAX_HISTORY_SIZE) {
                playerPositionHistory.remove(playerPositionHistory.firstKey());
            }
        }
    }

    public List<PlayerSpawnModel> spawnPlayers(List<String> playerNames) {
        List<PlayerSpawnModel> answer = playerNames.stream().map(
            playerName -> PlayerSpawnModel.builder()
                .playerName(playerName)
                .position(
                    new Vec3(
                        ThreadLocalRandom.current().nextFloat() * (MAP_MAX_X - 1),
                        0,
                        ThreadLocalRandom.current().nextFloat() * (MAP_MAX_Z - 1)
                    ).toArray()
                )
                .color(
                    new Vec3(
                        ThreadLocalRandom.current().nextFloat(),
                        ThreadLocalRandom.current().nextFloat(),
                        ThreadLocalRandom.current().nextFloat()
                    ).toArray()
                )
                .build()
        ).collect(Collectors.toList());

        answer.forEach(playerSpawnData -> {
            MMOPlayer player
                = (MMOPlayer) globalPlayerManager.getPlayer(playerSpawnData.getPlayerName());
            synchronized (player) {
                Vec3 initialPosition = new Vec3(
                    playerSpawnData.getPosition().get(0),
                    playerSpawnData.getPosition().get(1),
                    playerSpawnData.getPosition().get(2)
                );
                roomService.setPlayerPosition(player, initialPosition);
                player.setClientTimeTick(0);

                SortedMap<Integer, Vec3> playerPositionHistory
                    = positionHistoryByPlayerName.get(
                    player.getName());
                playerPositionHistory.put(0, initialPosition);
            }
        });

        return answer;
    }

    public AuthorizeHitModel authorizePlayerHit(
        String playerName,
        PlayerHitModel model
    ) {
        boolean isValidHit = checkHit(playerName, model);

        if (!isValidHit) {
            logger.warn("Player {} send invalid hit ", playerName);
            throw new InvalidPlayerHitException(playerName);
        }

        return AuthorizeHitModel.builder()
            .isValidHit(isValidHit)
            .playerName(playerName)
            .playerHit(model)
            .build();
    }

    private boolean checkHit(String playerName, PlayerHitModel model) {
        Vec3 attackPosition = new Vec3(
            model.getAttackPosition()[0],
            model.getAttackPosition()[1],
            model.getAttackPosition()[2]
        );
        int victimTick = model.getOtherClientTick();
        String victimName = model.getVictimName();

        // Roll back to get victim position at victimTick, a.k.a Lag compensation
        SortedMap<Integer, Vec3> victimPositionHistory
            = positionHistoryByPlayerName.get(victimName);
        Vec3 pastVictimPosition;
        if (victimPositionHistory.containsKey(victimTick)) {
            pastVictimPosition = victimPositionHistory.get(victimTick);
        } else {
            pastVictimPosition = victimPositionHistory.get(victimPositionHistory.firstKey());
            throw new IllegalStateException("Server doesn't contain victimTick");
        }

        // Check whether that position is near attackPosition
        if (pastVictimPosition.distance(attackPosition) > GameConstants.ATTACK_RANGE_UPPER_BOUND) {
            return false;
        }

        // Check if attackPosition is near my player's position
        MMOPlayer myPlayer = roomService.getPlayer(playerName);
        if (myPlayer.getPosition().distance(attackPosition)
            > GameConstants.HAMMER_DISTANCE_UPPER_BOUND) {
            return false;
        }

        return true;
    }

    public void resetPlayersPositionHistory(List<String> playerNames) {
        playerNames.forEach(playerName -> {
            if (!positionHistoryByPlayerName.containsKey(playerName)) {
                positionHistoryByPlayerName.put(
                    playerName,
                    Collections.synchronizedSortedMap(new TreeMap<>())
                );
            } else {
                positionHistoryByPlayerName.get(playerName).clear();
            }
        });
    }

    public StartGameModel startGame(EzyUser user) {
        MMORoom currentRoom = (MMORoom) roomService.getCurrentRoom(user.getName());
        currentRoom.setStatus(RoomStatus.PLAYING);
        List<String> playerNames = roomService.getRoomPlayerNames(currentRoom);
        resetPlayersPositionHistory(playerNames);
        List<PlayerSpawnModel> playerSpawns = spawnPlayers(playerNames);
        return StartGameModel.builder()
            .playerNames(playerNames)
            .playerSpawns(playerSpawns)
            .build();
    }
}
