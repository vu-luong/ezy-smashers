package com.youngmonkeys.app.game.shared;

import lombok.Getter;

public class PlayerInputData {
	@Getter
	private boolean[] keyInputs;
	
	public PlayerInputData(boolean[] keyInputs) {
		this.keyInputs = keyInputs;
	}
}
