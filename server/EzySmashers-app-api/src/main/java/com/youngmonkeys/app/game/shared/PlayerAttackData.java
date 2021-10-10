package com.youngmonkeys.app.game.shared;

import lombok.Data;

@Data
public class PlayerAttackData {
	float[] attackPosition;
	int time;
	
	public PlayerAttackData(float[] attackPosition, int time) {
		this.attackPosition = attackPosition;
		this.time = time;
	}
}
