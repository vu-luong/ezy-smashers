package com.youngmonkeys.app.game.shared;

import lombok.Data;

@Data
public class PlayerAttackData {
	float[] attackPosition;
	int myClientTick;
	int otherClientTick;
	String victimName;
	
	public PlayerAttackData(float[] attackPosition, int myClientTick, int otherClientTick, String victimName) {
		this.attackPosition = attackPosition;
		this.myClientTick = myClientTick;
		this.otherClientTick = otherClientTick;
		this.victimName = victimName;
	}
}
