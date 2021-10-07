package com.youngmonkeys.app.service.impl;

import com.tvd12.ezyfox.bean.annotation.EzyAutoBind;
import com.tvd12.ezyfox.bean.annotation.EzySingleton;
import com.tvd12.ezyfox.util.EzyLoggable;
import com.tvd12.gamebox.entity.MMOPlayer;
import com.tvd12.gamebox.math.Vec3;
import com.tvd12.gamebox.math.Vec3s;
import com.youngmonkeys.app.game.PlayerLogic;
import com.youngmonkeys.app.game.shared.PlayerInputData;
import com.youngmonkeys.app.game.shared.PlayerSpawnData;
import com.youngmonkeys.app.service.GamePlayService;
import com.youngmonkeys.app.service.RoomService;
import lombok.Setter;

import java.util.List;
import java.util.concurrent.ThreadLocalRandom;
import java.util.stream.Collectors;

@Setter
@EzySingleton
public class GamePlayServiceImpl extends EzyLoggable implements GamePlayService {
	
	@EzyAutoBind
	RoomService roomService;
	
	@Override
	public void handlePlayerInputData(String playerName, PlayerInputData inputData) {
		MMOPlayer player = roomService.getPlayer(playerName);
		synchronized (player) {
			Vec3 currentPosition = player.getPosition();
			Vec3 nextPosition = PlayerLogic.GetNextPosition(inputData, currentPosition);
			logger.info("next position = {}", nextPosition);
			player.setPosition(nextPosition);
			player.setClientTimeTick(inputData.getTime());
		}
	}
	
	@Override
	public List<PlayerSpawnData> spawnPlayers(List<String> playerNames) {
		return playerNames.stream().map(
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
	}
}
