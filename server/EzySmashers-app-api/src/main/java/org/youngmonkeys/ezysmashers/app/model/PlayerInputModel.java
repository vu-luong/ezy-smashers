package org.youngmonkeys.ezysmashers.app.model;

import lombok.Builder;
import lombok.Getter;

@Getter
@Builder
public class PlayerInputModel {
    private boolean[] inputs;
    private int time;
    private float[] rotation;
}
