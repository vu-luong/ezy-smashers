package com.youngmonkeys.app.service.impl;

import com.tvd12.ezyfox.bean.annotation.EzyAutoBind;
import com.tvd12.ezyfox.bean.annotation.EzySingleton;
import com.tvd12.ezyfoxserver.entity.EzyUser;
import com.tvd12.gamebox.entity.MMOPlayer;
import com.tvd12.gamebox.entity.MMOVirtualWorld;
import com.tvd12.gamebox.entity.NormalRoom;
import com.tvd12.gamebox.entity.Player;
import com.tvd12.gamebox.manager.DefaultPlayerManager;
import com.tvd12.gamebox.manager.DefaultRoomManager;
import com.tvd12.gamebox.manager.PlayerManager;
import com.tvd12.gamebox.manager.RoomManager;
import com.youngmonkeys.app.game.GameRoom;
import com.youngmonkeys.app.game.GameRoomFactory;
import com.youngmonkeys.app.service.GameService;
import lombok.Setter;

@Setter
@EzySingleton
public class GameServiceImpl implements GameService {
	
	@EzyAutoBind
	private MMOVirtualWorld mmoVirtualWorld;
	
	private final PlayerManager<Player> playerManager
			= new DefaultPlayerManager<>();
	
	private final RoomManager<NormalRoom> roomManager
			= new DefaultRoomManager<>();
	
	@Override
	public NormalRoom removePlayer(String username) {
		Player player = playerManager.getPlayer(username);
		if (player == null) {
			return null;
		}
		NormalRoom room = roomManager.getRoom(player.getCurrentRoomId());
		if (room != null) {
			synchronized (room) {
				room.removePlayer(player);
				if (room.getPlayerManager().isEmpty()) {
					roomManager.removeRoom(room);
				}
			}
		}
		
		playerManager.removePlayer(player);
		return room;
	}
	
	@Override
	public void addPlayer(MMOPlayer player) {
		playerManager.addPlayer(player);
	}
	
	@Override
	public GameRoom newGameRoom(EzyUser user) {
		MMOPlayer player = getPlayer(user.getName());
		GameRoom room = new GameRoomFactory().newGameRoom();
		room.addUser(user, player);
		
		mmoVirtualWorld.addRoom(room);
		roomManager.addRoom(room);
		return room;
	}
	
	@Override
	public MMOPlayer getPlayer(String playerName) {
		return (MMOPlayer) playerManager.getPlayer(playerName);
	}
}
