package com.youngmonkeys.app.service.impl;

import com.tvd12.ezyfox.bean.annotation.EzyAutoBind;
import com.tvd12.ezyfox.bean.annotation.EzySingleton;
import com.tvd12.ezyfoxserver.entity.EzyUser;
import com.tvd12.gamebox.entity.MMOPlayer;
import com.tvd12.gamebox.entity.NormalRoom;
import com.youngmonkeys.app.config.AppConfig;
import com.youngmonkeys.app.service.GameService;
import com.youngmonkeys.app.service.LobbyService;
import lombok.Setter;

import java.util.List;

@Setter
@EzySingleton
public class LobbyServiceImpl implements LobbyService {
	
	@EzyAutoBind
	private NormalRoom lobbyRoom;
	
	@EzyAutoBind
	private GameService gameService;
	
	@Override
	public void addUser(EzyUser user) {
		MMOPlayer player = new MMOPlayer(user.getName());
		synchronized (lobbyRoom) {
			if (lobbyRoom.getPlayerManager().containsPlayer(player)) {
				return;
			}
			lobbyRoom.addUser(user, player);
			player.setCurrentRoomId(lobbyRoom.getId());
		}
		
		gameService.addPlayer(player);
	}
	
	@Override
	public List<String> getPlayerNames() {
		synchronized (lobbyRoom) {
			return lobbyRoom.getPlayerManager().getPlayerNames();
		}
	}
	
	@Override
	public long getRoomId() {
		synchronized (lobbyRoom) {
			return lobbyRoom.getId();
		}
	}
}
