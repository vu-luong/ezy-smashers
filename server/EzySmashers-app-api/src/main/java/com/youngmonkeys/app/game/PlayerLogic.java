package com.youngmonkeys.app.game;

import com.tvd12.gamebox.math.Vec3;
import com.youngmonkeys.app.game.constant.GameConstants;
import com.youngmonkeys.app.game.shared.PlayerInputData;
import com.youngmonkeys.app.game.utils.InputUtils;
import com.youngmonkeys.app.game.utils.Vec3Utils;

public class PlayerLogic {
	
	public static float velocity = 6f;
	
	public static Vec3 GetNextPosition(PlayerInputData inputData, Vec3 currentPosition) {
		boolean upInput = inputData.getInputs()[0];
		boolean leftInput = inputData.getInputs()[1];
		boolean downInput = inputData.getInputs()[2];
		boolean rightInput = inputData.getInputs()[3];
		
		Vec3 movement = InputUtils.ComputeMovementFromInput(upInput, leftInput, downInput, rightInput);
		
		// moveDirection = Vec3Utils.forward * movement.z + Vec3Utils.right * movement.x
		Vec3 moveDirection = new Vec3(Vec3Utils.forward);
		moveDirection.multiply(movement.z);
		Vec3 temp = new Vec3(Vec3Utils.right);
		temp.multiply(movement.x);
		moveDirection.add(temp);
		
		// moveVector = moveDirection * fixedDeltaTime * velocity
		Vec3 moveVector = new Vec3(moveDirection);
		moveVector.multiply(GameConstants.CLIENT_FIXED_DELTA_TIME * velocity);
		
		// nextPosition = currentPosition + moveVector
		Vec3 nextPosition = new Vec3(currentPosition);
		nextPosition.add(moveVector);
		
		return nextPosition;
	}
	
}
