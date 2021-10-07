package com.youngmonkeys.app.controller;

import com.tvd12.ezyfox.bean.annotation.EzyAutoBind;
import com.tvd12.ezyfox.core.annotation.EzyDoHandle;
import com.tvd12.ezyfox.core.annotation.EzyRequestController;
import com.tvd12.ezyfox.io.EzyLists;
import com.tvd12.ezyfox.util.EzyLoggable;
import com.tvd12.ezyfoxserver.entity.EzyUser;
import com.tvd12.ezyfoxserver.support.factory.EzyResponseFactory;
import com.tvd12.gamebox.entity.Player;
import com.youngmonkeys.app.constant.Commands;
import com.youngmonkeys.app.game.GameRoom;
import com.youngmonkeys.app.game.shared.PlayerInputData;
import com.youngmonkeys.app.request.JoinMMORoomRequest;
import com.youngmonkeys.app.request.PlayerInputDataRequest;
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
		GameRoom room = gameService.newGameRoom(user);
		
		responseFactory.newObjectResponse()
				.command(Commands.CREATE_MMO_ROOM)
				.param("roomId", room.getId())
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
	
	@EzyDoHandle(Commands.JOIN_MMO_ROOM)
	public void joinMMORoom(EzyUser user, JoinMMORoomRequest request) {
		logger.info("user {} join room {}", user.getName(), request.getRoomId());
		long roomId = request.getRoomId();
		GameRoom room = gameService.playerJoinMMORoom(user.getName(), roomId);
		List<String> playerNames = gameService.getRoomPlayerNames(room);
		
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
		GameRoom currentRoom = (GameRoom) gameService.getCurrentRoom(user.getName());
		List<String> playerNames = gameService.getRoomPlayerNames(currentRoom);
		
		responseFactory.newObjectResponse()
				.command(Commands.START_GAME)
				.usernames(playerNames)
				.execute();
	}
	
	@EzyDoHandle(Commands.PLAYER_INPUT_DATA)
	public void handlePlayerInputData(EzyUser user, PlayerInputDataRequest request) {
		logger.info("user {} send input data {}", user.getName(), request);
		gameService.handlePlayerInputData(user.getName(), new PlayerInputData(request.getK(), request.getT()));
	}
}
