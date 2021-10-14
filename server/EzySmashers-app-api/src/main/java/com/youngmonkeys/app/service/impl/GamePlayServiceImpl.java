package com.youngmonkeys.app.service.impl;

import com.tvd12.ezyfox.bean.annotation.EzyAutoBind;
import com.tvd12.ezyfox.bean.annotation.EzySingleton;
import com.tvd12.ezyfox.util.EzyLoggable;
import com.tvd12.gamebox.entity.MMOPlayer;
import com.tvd12.gamebox.entity.Player;
import com.tvd12.gamebox.manager.PlayerManager;
import com.tvd12.gamebox.math.Vec3;
import com.tvd12.gamebox.math.Vec3s;
import com.youngmonkeys.app.game.GameRoom;
import com.youngmonkeys.app.game.PlayerLogic;
import com.youngmonkeys.app.game.shared.PlayerAttackData;
import com.youngmonkeys.app.game.shared.PlayerInputData;
import com.youngmonkeys.app.game.shared.PlayerSpawnData;
import com.youngmonkeys.app.service.GamePlayService;
import com.youngmonkeys.app.service.RoomService;
import lombok.Setter;

import java.util.ArrayList;
import java.util.List;
import java.util.concurrent.ThreadLocalRandom;
import java.util.stream.Collectors;

@Setter
@EzySingleton
public class GamePlayServiceImpl extends EzyLoggable implements GamePlayService {
	
	@EzyAutoBind
	RoomService roomService;
	
	@EzyAutoBind
	private PlayerManager<Player> globalPlayerManager;
	
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
		}
	}
	
	@Override
	public List<PlayerSpawnData> spawnPlayers(List<String> playerNames) {
		List<PlayerSpawnData> answer = playerNames.stream().map(
				playerName -> new PlayerSpawnData(
						playerName,
						Vec3s.toArray(
								new Vec3(
										ThreadLocalRandom.current().nextFloat() * 10,
										0,
										ThreadLocalRandom.current().nextFloat() * 10
								)
						)
				)
		).collect(Collectors.toList());
		
		answer.forEach(playerSpawnData -> {
			MMOPlayer player = (MMOPlayer) globalPlayerManager.getPlayer(playerSpawnData.getPlayerName());
			synchronized (player) {
				player.setPosition(
						new Vec3(
								playerSpawnData.getPosition().get(0),
								playerSpawnData.getPosition().get(1),
								playerSpawnData.getPosition().get(2)
						)
				);
			}
		});
		
		return answer;
	}
	
	@Override
	public void authorizeAttack(String playerName, PlayerAttackData playerAttackData) {
		Vec3 attackPosition = new Vec3(
				playerAttackData.getAttackPosition()[0],
				playerAttackData.getAttackPosition()[1],
				playerAttackData.getAttackPosition()[2]
		);
		int myTick = playerAttackData.getMyClientTick();
		int victimTick = playerAttackData.getOtherClientTick();
		// TODO: 1. roll back to get victim position at victimTick // Lag compensation
		// TODO: 2. Check whether that position is near attackPosition
		// TODO: 3. Check if attackPosition is near my player's position
		GameRoom currentRoom = (GameRoom) roomService.getCurrentRoom(playerName);
		List<Player> players = roomService.getRoomPlayers(currentRoom);
		
//		playerBeingAttacked.clear();
//		for (Player player : players) {
//			logger.info("Player {} distance: {}", player.getName(), ((MMOPlayer) player).getPosition().distance(attackPosition));
//			if (((MMOPlayer) player).getPosition().distance(attackPosition) < 1.0f) {
//				logger.info("Player {} is being attacked by {}", player.getName(), playerName);
//				playerBeingAttacked.add(player.getName());
//			}
//		}
//		playerNames.addAll(players.stream().map(Player::getName).collect(Collectors.toList()));
	}
}
