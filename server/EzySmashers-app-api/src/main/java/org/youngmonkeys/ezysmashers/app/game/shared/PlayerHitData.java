package org.youngmonkeys.ezysmashers.app.game.shared;

import lombok.Data;

@Data
public class PlayerHitData {
	float[] attackPosition;
	int myClientTick;
	int otherClientTick;
	String victimName;
	
	public PlayerHitData(float[] attackPosition, int myClientTick, int otherClientTick, String victimName) {
		this.attackPosition = attackPosition;
		this.myClientTick = myClientTick;
		this.otherClientTick = otherClientTick;
		this.victimName = victimName;
	}
}
