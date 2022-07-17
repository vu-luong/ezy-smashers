package org.youngmonkeys.ezysmashers.app.game.shared;

import com.tvd12.ezyfox.binding.annotation.EzyObjectBinding;
import com.tvd12.ezyfox.entity.EzyArray;
import lombok.Data;

@Data
@EzyObjectBinding
public class PlayerSpawnData {
	String playerName;
	EzyArray position;
	EzyArray color;
	
	public PlayerSpawnData(String playerName, EzyArray position, EzyArray color) {
		this.playerName = playerName;
		this.position = position;
		this.color = color;
	}
}
