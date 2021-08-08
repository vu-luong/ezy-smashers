package com.youngmonkeys.app.service;

import com.tvd12.ezyfoxserver.entity.EzyUser;

import java.util.List;

public interface LobbyService {
	void addUser(EzyUser user);
	
	List<String> getPlayerNames();
	
	long getRoomId();
}
