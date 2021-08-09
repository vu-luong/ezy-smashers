package com.youngmonkeys.app.controller;

import com.tvd12.ezyfox.bean.annotation.EzyAutoBind;
import com.tvd12.ezyfox.core.annotation.EzyDoHandle;
import com.tvd12.ezyfox.core.annotation.EzyRequestController;
import com.tvd12.ezyfox.io.EzyLists;
import com.tvd12.ezyfox.util.EzyLoggable;
import com.tvd12.ezyfoxserver.entity.EzyUser;
import com.tvd12.ezyfoxserver.support.factory.EzyResponseFactory;
import com.youngmonkeys.app.constant.Commands;
import com.youngmonkeys.app.game.GameRoom;
import com.youngmonkeys.app.service.GameService;
import com.youngmonkeys.app.service.LobbyService;
import lombok.Setter;

import java.util.List;

@Setter
@EzyRequestController
public class GameRequestController extends EzyLoggable {
	
	@EzyAutoBind
	private LobbyService lobbyService;
	
	@EzyAutoBind
	private GameService gameService;
	
	@EzyAutoBind
	private EzyResponseFactory responseFactory;
	
	@EzyDoHandle(Commands.JOIN_LOBBY)
	public void joinLobby(EzyUser user) {
		logger.info("user {} join lobby room", user);
		List<String> playerNames;
		long lobbyRoomId;
		
		synchronized (lobbyService) {
			lobbyService.addUser(user);
			playerNames = lobbyService.getPlayerNames();
			lobbyRoomId = lobbyService.getRoomId();
		}
		
		responseFactory.newObjectResponse()
				.command(Commands.JOIN_LOBBY)
				.param("lobbyRoomId", lobbyRoomId)
				.param("playerNames", playerNames)
				.user(user)
				.execute();
		
		responseFactory.newObjectResponse()
				.command(Commands.PLAYER_JOINED_LOBBY)
				.param("playerName", user.getName())
				.usernames(EzyLists.filter(playerNames, it -> !it.equals(user.getName())))
				.execute();
	}
	
	@EzyDoHandle(Commands.CREATE_MMO_ROOM)
	public void createMMORoom(EzyUser user) {
		logger.info("user {} create an MMO room", user);
		GameRoom room;
		synchronized (gameService) {
			room = gameService.newGameRoom(user);
		}
		
		responseFactory.newObjectResponse()
				.command(Commands.CREATE_MMO_ROOM)
				.param("roomId", room.getId())
				.param("master", true)
				.user(user)
				.execute();
	}
}
