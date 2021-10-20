package com.youngmonkeys.app.service;

import com.tvd12.gamebox.util.ReadOnlySet;

public interface LobbyService {
	void addNewPlayer(String playerName);
	
	ReadOnlySet<String> getPlayerNames();
	
	long getRoomId();
}
