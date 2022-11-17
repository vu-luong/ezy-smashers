package org.youngmonkeys.ezysmashers.app.utils;

import com.tvd12.gamebox.math.Vec3;
import org.youngmonkeys.ezysmashers.app.constant.GameConstants;
import org.youngmonkeys.ezysmashers.app.model.PlayerInputModel;

import static org.youngmonkeys.ezysmashers.app.constant.PlayerConstants.PLAYER_VELOCITY;

public final class PlayerUtils {

    public static Vec3 getNextPosition(PlayerInputModel model, Vec3 currentPosition) {
        boolean upInput = model.getInputs()[0];
        boolean leftInput = model.getInputs()[1];
        boolean downInput = model.getInputs()[2];
        boolean rightInput = model.getInputs()[3];

        Vec3 movement = InputUtils.computeMovementFromInput(
            upInput,
            leftInput,
            downInput,
            rightInput
        );

        // moveDirection = Vec3Utils.forward * movement.z + Vec3Utils.right * movement.x
        Vec3 moveDirection = new Vec3(Vec3.forward);
        moveDirection.multiply(movement.z);
        Vec3 temp = new Vec3(Vec3.right);
        temp.multiply(movement.x);
        moveDirection.add(temp);

        // moveVector = moveDirection * fixedDeltaTime * velocity
        Vec3 moveVector = new Vec3(moveDirection);
        moveVector.multiply(GameConstants.CLIENT_FIXED_DELTA_TIME * PLAYER_VELOCITY);

        // nextPosition = currentPosition + moveVector
        Vec3 nextPosition = new Vec3(currentPosition);
        nextPosition.add(moveVector);

        return nextPosition;
    }
}
