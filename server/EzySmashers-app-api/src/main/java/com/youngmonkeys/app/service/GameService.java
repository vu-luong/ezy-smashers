package com.youngmonkeys.app.service;

import com.tvd12.gamebox.entity.MMOPlayer;
import com.tvd12.gamebox.entity.NormalRoom;

public interface GameService {
	NormalRoom removePlayer(String username);
	
	void addPlayer(MMOPlayer player);
}
