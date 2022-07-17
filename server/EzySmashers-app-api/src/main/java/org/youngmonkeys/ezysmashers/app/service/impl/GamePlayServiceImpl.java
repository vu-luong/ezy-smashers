package org.youngmonkeys.ezysmashers.app.service.impl;

import com.tvd12.ezyfox.bean.annotation.EzyAutoBind;
import com.tvd12.ezyfox.bean.annotation.EzySingleton;
import com.tvd12.ezyfox.util.EzyLoggable;
import com.tvd12.gamebox.entity.MMOPlayer;
import com.tvd12.gamebox.entity.Player;
import com.tvd12.gamebox.manager.PlayerManager;
import com.tvd12.gamebox.math.Vec3;
import org.youngmonkeys.ezysmashers.app.game.PlayerLogic;
import org.youngmonkeys.ezysmashers.app.game.constant.GameConstants;
import org.youngmonkeys.ezysmashers.app.game.shared.PlayerHitData;
import org.youngmonkeys.ezysmashers.app.game.shared.PlayerInputData;
import org.youngmonkeys.ezysmashers.app.game.shared.PlayerSpawnData;
import org.youngmonkeys.ezysmashers.app.service.GamePlayService;
import org.youngmonkeys.ezysmashers.app.service.RoomService;
import lombok.Setter;

import java.util.*;
import java.util.concurrent.ThreadLocalRandom;
import java.util.stream.Collectors;

@Setter
@EzySingleton
public class GamePlayServiceImpl extends EzyLoggable implements GamePlayService {

	@EzyAutoBind
	RoomService roomService;

	@EzyAutoBind
	private PlayerManager<Player> globalPlayerManager;

	/**
	 * Map playerName to playerPositionHistory
	 */
	private Map<String, SortedMap<Integer, Vec3>> globalPlayersPositionHistory = new HashMap<>();

	@Override
	public void handlePlayerInputData(String playerName, PlayerInputData inputData, float[] nextRotation) {
		MMOPlayer player = roomService.getPlayer(playerName);
		synchronized (player) {
			Vec3 currentPosition = player.getPosition();
			Vec3 nextPosition = PlayerLogic.GetNextPosition(inputData, currentPosition);
			logger.info("next position = {}", nextPosition);
			player.setPosition(nextPosition);
			player.setRotation(nextRotation[0], nextRotation[1], nextRotation[2]);
			player.setClientTimeTick(inputData.getTime());

			SortedMap<Integer, Vec3> playerPositionHistory = globalPlayersPositionHistory.get(playerName);
			playerPositionHistory.put(inputData.getTime(), nextPosition);
			if (playerPositionHistory.size() > GameConstants.MAX_HISTORY_SIZE) {
				playerPositionHistory.remove(playerPositionHistory.firstKey());
			}
		}
	}

	@Override
	public List<PlayerSpawnData> spawnPlayers(List<String> playerNames) {
		List<PlayerSpawnData> answer = playerNames.stream().map(
				playerName -> new PlayerSpawnData(
						playerName,
                        new Vec3(
                                ThreadLocalRandom.current().nextFloat() * 10,
                                0,
                                ThreadLocalRandom.current().nextFloat() * 10
                        ).toArray(),
                        new Vec3(
                                ThreadLocalRandom.current().nextFloat(),
                                ThreadLocalRandom.current().nextFloat(),
                                ThreadLocalRandom.current().nextFloat()
                        ).toArray()
				)
		).collect(Collectors.toList());

		answer.forEach(playerSpawnData -> {
			MMOPlayer player = (MMOPlayer) globalPlayerManager.getPlayer(playerSpawnData.getPlayerName());
			synchronized (player) {
				Vec3 initialPosition = new Vec3(
						playerSpawnData.getPosition().get(0),
						playerSpawnData.getPosition().get(1),
						playerSpawnData.getPosition().get(2)
				);
				player.setPosition(
						initialPosition
				);
				player.setClientTimeTick(0);

				SortedMap<Integer, Vec3> playerPositionHistory = globalPlayersPositionHistory.get(player.getName());
				playerPositionHistory.put(0, initialPosition);
			}
		});

		return answer;
	}

	@Override
	public boolean authorizeHit(String playerName, PlayerHitData playerHitData) {
		Vec3 attackPosition = new Vec3(
				playerHitData.getAttackPosition()[0],
				playerHitData.getAttackPosition()[1],
				playerHitData.getAttackPosition()[2]
		);
		int victimTick = playerHitData.getOtherClientTick();
		String victimName = playerHitData.getVictimName();

		// Roll back to get victim position at victimTick, a.k.a Lag compensation
		SortedMap<Integer, Vec3> victimPositionHistory = globalPlayersPositionHistory.get(victimName);
		Vec3 pastVictimPosition;
		if (victimPositionHistory.containsKey(victimTick)) {
			pastVictimPosition = victimPositionHistory.get(victimTick);
		} else {
			pastVictimPosition = victimPositionHistory.get(victimPositionHistory.firstKey());
			throw new IllegalStateException("Server doesn't contain victimTick");
		}

		// Check whether that position is near attackPosition
		if (pastVictimPosition.distance(attackPosition) > GameConstants.ATTACK_RANGE_UPPER_BOUND) {
			return false;
		}

		// Check if attackPosition is near my player's position
		MMOPlayer myPlayer = roomService.getPlayer(playerName);
		if (myPlayer.getPosition().distance(attackPosition) > GameConstants.HAMMER_DISTANCE_UPPER_BOUND) {
			return false;
		}

		return true;
	}

	@Override
	public void resetPlayersPositionHistory(List<String> playerNames) {
		playerNames.forEach(playerName -> {
			if (!globalPlayersPositionHistory.containsKey(playerName)) {
				globalPlayersPositionHistory.put(playerName,
						Collections.synchronizedSortedMap(new TreeMap<>()));
			} else {
				globalPlayersPositionHistory.get(playerName).clear();
			}
		});
	}
}
