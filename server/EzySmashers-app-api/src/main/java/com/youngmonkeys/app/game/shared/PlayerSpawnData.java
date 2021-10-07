package com.youngmonkeys.app.game.shared;

import com.tvd12.ezyfox.binding.annotation.EzyObjectBinding;
import com.tvd12.ezyfox.entity.EzyArray;
import lombok.Data;

@Data
@EzyObjectBinding
public class PlayerSpawnData {
	String playerName;
	EzyArray position;
	
	public PlayerSpawnData(String playerName, EzyArray position) {
		this.playerName = playerName;
		this.position = position;
	}
}
