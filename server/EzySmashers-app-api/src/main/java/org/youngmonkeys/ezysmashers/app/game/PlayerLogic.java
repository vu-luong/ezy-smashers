package org.youngmonkeys.ezysmashers.app.game;

import com.tvd12.gamebox.math.Vec3;
import org.youngmonkeys.ezysmashers.app.game.constant.GameConstants;
import org.youngmonkeys.ezysmashers.app.game.shared.PlayerInputData;
import org.youngmonkeys.ezysmashers.app.game.utils.InputUtils;

public class PlayerLogic {
	
	public static float velocity = 6f;
	
	public static Vec3 GetNextPosition(PlayerInputData inputData, Vec3 currentPosition) {
		boolean upInput = inputData.getInputs()[0];
		boolean leftInput = inputData.getInputs()[1];
		boolean downInput = inputData.getInputs()[2];
		boolean rightInput = inputData.getInputs()[3];
		
		Vec3 movement = InputUtils.computeMovementFromInput(upInput, leftInput, downInput, rightInput);
		
		// moveDirection = Vec3Utils.forward * movement.z + Vec3Utils.right * movement.x
		Vec3 moveDirection = new Vec3(Vec3.forward);
		moveDirection.multiply(movement.z);
		Vec3 temp = new Vec3(Vec3.right);
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
