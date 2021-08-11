package com.youngmonkeys.app.service.impl;

import com.tvd12.ezyfox.bean.annotation.EzyAutoBind;
import com.tvd12.ezyfox.bean.annotation.EzySingleton;
import com.tvd12.ezyfoxserver.entity.EzyUser;
import com.tvd12.gamebox.entity.*;
import com.tvd12.gamebox.manager.PlayerManager;
import com.tvd12.gamebox.manager.RoomManager;
import com.youngmonkeys.app.game.GameRoom;
import com.youngmonkeys.app.game.GameRoomFactory;
import com.youngmonkeys.app.service.GameService;
import lombok.Setter;

import java.util.Comparator;
import java.util.List;
import java.util.stream.Collectors;

@Setter
@EzySingleton
@SuppressWarnings("unchecked")
public class GameServiceImpl implements GameService {
	
	@EzyAutoBind
	private MMOVirtualWorld mmoVirtualWorld;
	
	@EzyAutoBind
	private GameRoomFactory gameRoomFactory;
	
	@EzyAutoBind
	private PlayerManager<Player> globalPlayerManager;
	
	@EzyAutoBind
	private RoomManager<NormalRoom> globalRoomManager;
	
	@EzyAutoBind
	private NormalRoom lobbyRoom;
	
	@Override
	public NormalRoom removePlayer(String username) {
		Player player = globalPlayerManager.getPlayer(username);
		if (player == null) {
			return null;
		}
		NormalRoom room = globalRoomManager.getRoom(player.getCurrentRoomId());
		if (room != null) {
			synchronized (room) {
				room.removePlayer(player);
				if (room.getPlayerManager().isEmpty()) {
					globalRoomManager.removeRoom(room);
				}
			}
		}
		
		globalPlayerManager.removePlayer(player);
		return room;
	}
	
	@Override
	public void addPlayer(MMOPlayer player) {
		globalPlayerManager.addPlayer(player);
	}
	
	@Override
	public GameRoom newGameRoom(EzyUser user) {
		MMOPlayer player = getPlayer(user.getName());
		GameRoom room = gameRoomFactory.newGameRoom();
//		room.addUser(user, player);
		room.addPlayer(player);
		player.setCurrentRoomId(room.getId());
		
		this.addRoom(room);
		return room;
	}
	
	@Override
	public MMOPlayer getPlayer(String playerName) {
		return (MMOPlayer) globalPlayerManager.getPlayer(playerName);
	}
	
	@Override
	public List<String> getRoomPlayerNames(NormalRoom room) {
		synchronized (room) {
			return room.getPlayerManager().getPlayerNames();
		}
	}
	
	@Override
	public void addRoom(NormalRoom room) {
		if (room instanceof MMORoom) {
			synchronized (mmoVirtualWorld) {
				mmoVirtualWorld.addRoom((MMORoom) room);
			}
		}
		globalRoomManager.addRoom(room);
	}
	
	@Override
	public List<Long> getMMORoomIdList() {
		return globalRoomManager
				.getRoomList()
				.stream()
				.filter(room -> !room.getName().equals(lobbyRoom.getName()))
				.map(Room::getId)
				.collect(Collectors.toList());
	}
	
	@Override
	public NormalRoom getCurrentRoom(String playerName) {
		Player player = globalPlayerManager.getPlayer(playerName);
		long currentRoomId = player.getCurrentRoomId();
		return globalRoomManager.getRoom(currentRoomId);
	}
	
	@Override
	public Player getMaster(NormalRoom room) {
		synchronized (room) {
			if (room instanceof GameRoom) {
				return ((GameRoom) room).getMaster();
			} else {
				return null;
			}
		}
	}
}
