package com.youngmonkeys.app.service.impl;

import com.tvd12.ezyfox.bean.annotation.EzyAutoBind;
import com.tvd12.ezyfox.bean.annotation.EzySingleton;
import com.tvd12.ezyfoxserver.entity.EzyUser;
import com.tvd12.gamebox.entity.MMOPlayer;
import com.tvd12.gamebox.entity.NormalRoom;
import com.youngmonkeys.app.service.LobbyService;
import lombok.Setter;

import java.util.List;

@Setter
@EzySingleton
public class LobbyServiceImpl implements LobbyService {
	
	@EzyAutoBind
	private NormalRoom lobbyRoom;
	
	@Override
	public void addUser(EzyUser user) {
		MMOPlayer player = new MMOPlayer(user.getName());
		lobbyRoom.addUser(user, player);
		player.setCurrentRoomId(lobbyRoom.getId());
	}
	
	@Override
	public List<String> getPlayerNames() {
		return lobbyRoom.getPlayerManager().getPlayerNames();
	}
	
	@Override
	public long getRoomId() {
		return lobbyRoom.getId();
	}
}
