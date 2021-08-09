package com.youngmonkeys.app.service;

import com.tvd12.ezyfoxserver.entity.EzyUser;
import com.tvd12.gamebox.entity.MMOPlayer;
import com.tvd12.gamebox.entity.NormalRoom;
import com.youngmonkeys.app.game.GameRoom;

public interface GameService {
	NormalRoom removePlayer(String username);
	
	void addPlayer(MMOPlayer player);
	
	
	GameRoom newGameRoom(EzyUser user);
	
	MMOPlayer getPlayer(String playerName);
}
