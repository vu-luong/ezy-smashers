package org.youngmonkeys.ezysmashers.app.controller;

import com.tvd12.ezyfox.core.annotation.EzyDoHandle;
import com.tvd12.ezyfox.core.annotation.EzyRequestController;
import com.tvd12.ezyfox.util.EzyLoggable;
import com.tvd12.ezyfoxserver.entity.EzyUser;
import com.tvd12.ezyfoxserver.support.factory.EzyResponseFactory;
import com.tvd12.gamebox.entity.MMORoom;
import lombok.AllArgsConstructor;
import org.youngmonkeys.ezysmashers.app.constant.Commands;
import org.youngmonkeys.ezysmashers.app.converter.ModelToResponseConverter;
import org.youngmonkeys.ezysmashers.app.converter.RequestToModelConverter;
import org.youngmonkeys.ezysmashers.app.model.JoinedMMORoomModel;
import org.youngmonkeys.ezysmashers.app.model.MMORoomPlayerNamesModel;
import org.youngmonkeys.ezysmashers.app.request.JoinMMORoomRequest;
import org.youngmonkeys.ezysmashers.app.service.LobbyService;
import org.youngmonkeys.ezysmashers.app.service.RoomService;

import java.util.List;

import static org.youngmonkeys.ezysmashers.app.constant.RoomConstants.FIELD_LOBBY_ROOM_ID;

@AllArgsConstructor
@EzyRequestController
public class RoomController extends EzyLoggable {

    private final LobbyService lobbyService;
    private final RoomService roomService;
    private final EzyResponseFactory responseFactory;
    private final ModelToResponseConverter modelToResponseConverter;
    private final RequestToModelConverter requestToModelConverter;

    @EzyDoHandle(Commands.JOIN_LOBBY)
    public void joinLobby(EzyUser user) {
        logger.info("user {} join lobby room", user);

        lobbyService.addNewPlayer(user.getName());

        responseFactory.newObjectResponse()
            .command(Commands.JOIN_LOBBY)
            .param(FIELD_LOBBY_ROOM_ID, lobbyService.getRoomId())
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

        MMORoomPlayerNamesModel model = roomService.getMMORoomPlayerNames(user.getName());

        modelToResponseConverter.toResponse(model)
            .command(Commands.GET_MMO_ROOM_PLAYERS)
            .user(user)
            .execute();
    }

    @EzyDoHandle(Commands.JOIN_MMO_ROOM)
    public void joinMMORoom(EzyUser user, JoinMMORoomRequest request) {
        logger.info("user {} join room {}", user.getName(), request.getRoomId());

        JoinedMMORoomModel model = roomService.playerJoinMMORoom(
            requestToModelConverter.toModel(user.getName(), request.getRoomId())
        );

        modelToResponseConverter.toResponse(user, model)
            .command(Commands.JOIN_MMO_ROOM)
            .execute();

        modelToResponseConverter.toResponse(user.getName(), model)
            .command(Commands.ANOTHER_JOIN_MMO_ROOM)
            .execute();
    }
}
