package org.youngmonkeys.ezysmashers.app.service;

import org.youngmonkeys.ezysmashers.app.game.shared.PlayerHitData;
import org.youngmonkeys.ezysmashers.app.game.shared.PlayerInputData;
import org.youngmonkeys.ezysmashers.app.game.shared.PlayerSpawnData;

import java.util.List;

public interface GamePlayService {
	void handlePlayerInputData(String playerName, PlayerInputData inputData, float[] nextRotation);
	
	List<PlayerSpawnData> spawnPlayers(List<String> playerNames);
	
	boolean authorizeHit(String playerName, PlayerHitData playerHitData);
	
	void resetPlayersPositionHistory(List<String> playerNames);
}
