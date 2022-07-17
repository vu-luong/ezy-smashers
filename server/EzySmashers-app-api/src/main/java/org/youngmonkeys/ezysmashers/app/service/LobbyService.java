package org.youngmonkeys.ezysmashers.app.service;


import java.util.List;

public interface LobbyService {
	void addNewPlayer(String playerName);

	List<String> getPlayerNames();

	long getRoomId();
}
