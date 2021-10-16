package com.youngmonkeys.app.service;

import com.tvd12.ezyfoxserver.entity.EzyUser;

import java.util.List;

public interface LobbyService {
	void addNewPlayer(String playerName);
	
	List<String> getPlayerNames();
	
	long getRoomId();
}
