package com.youngmonkeys.app.service;

import com.youngmonkeys.app.game.shared.PlayerInputData;
import com.youngmonkeys.app.game.shared.PlayerSpawnData;

import java.util.List;

public interface GamePlayService {
	void handlePlayerInputData(String playerName, PlayerInputData inputData);
	
	List<PlayerSpawnData> spawnPlayers(List<String> playerNames);
}
