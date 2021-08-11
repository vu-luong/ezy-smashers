package com.youngmonkeys.app.controller;

import com.tvd12.ezyfox.bean.annotation.EzyAutoBind;
import com.tvd12.ezyfox.core.annotation.EzyDoHandle;
import com.tvd12.ezyfox.core.annotation.EzyRequestController;
import com.tvd12.ezyfox.io.EzyLists;
import com.tvd12.ezyfox.util.EzyLoggable;
import com.tvd12.ezyfoxserver.entity.EzyUser;
import com.tvd12.ezyfoxserver.support.factory.EzyResponseFactory;
import com.tvd12.gamebox.entity.MMORoom;
import com.tvd12.gamebox.entity.NormalRoom;
import com.tvd12.gamebox.entity.Player;
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
		
		lobbyService.addUser(user);
		List<String> playerNames = lobbyService.getPlayerNames();
		long lobbyRoomId = lobbyService.getRoomId();
		
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
		GameRoom room = gameService.newGameRoom(user);
		
		responseFactory.newObjectResponse()
				.command(Commands.CREATE_MMO_ROOM)
				.param("roomId", room.getId())
				.param("master", true)
				.user(user)
				.execute();
	}
	
	@EzyDoHandle(Commands.GET_MMO_ROOM_ID_LIST)
	public void getMMORoomIdList(EzyUser user) {
		logger.info("user {} get MMO room list", user);
		List<Long> mmoRoomIdList = gameService.getMMORoomIdList();
		responseFactory.newArrayResponse()
				.command(Commands.GET_MMO_ROOM_ID_LIST)
				.param(mmoRoomIdList)
				.user(user)
				.execute();
	}
	
	@EzyDoHandle(Commands.GET_MMO_ROOM_PLAYERS)
	public void getMMORoomPlayers(EzyUser user) {
		logger.info("user {} getMMORoomPlayers", user);
		GameRoom currentRoom = (GameRoom) gameService.getCurrentRoom(user.getName());
		List<String> players = gameService.getRoomPlayerNames(currentRoom);
		Player master = gameService.getMaster(currentRoom);
		
		responseFactory.newObjectResponse()
				.command(Commands.GET_MMO_ROOM_PLAYERS)
				.param("players", players)
				.param("master", master.getName())
				.user(user)
				.execute();
	}
}
