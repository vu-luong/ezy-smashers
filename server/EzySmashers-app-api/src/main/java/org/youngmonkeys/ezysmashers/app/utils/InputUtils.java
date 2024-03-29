package org.youngmonkeys.ezysmashers.app.utils;

import com.tvd12.gamebox.math.Vec3;

public final class InputUtils {
    
    private InputUtils() {}

    public static Vec3 computeMovementFromInput(
        boolean upInput,
        boolean leftInput,
        boolean downInput,
        boolean rightInput
    ) {
        Vec3 answer = new Vec3();
        if (upInput) {
            answer.add(Vec3.FORWARD);
        }
        if (leftInput) {
            answer.add(Vec3.LEFT);
        }
        if (downInput) {
            answer.add(Vec3.BACKWARD);
        }
        if (rightInput) {
            answer.add(Vec3.RIGHT);
        }
        return answer;
    }
}
