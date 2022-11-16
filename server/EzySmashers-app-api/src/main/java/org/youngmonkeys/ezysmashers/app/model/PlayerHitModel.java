package org.youngmonkeys.ezysmashers.app.model;

import lombok.Builder;
import lombok.Getter;

@Getter
@Builder
public class PlayerHitModel {
    private float[] attackPosition;
    private int myClientTick;
    private int otherClientTick;
    private String victimName;

    public PlayerHitModel(
        float[] attackPosition,
        int myClientTick,
        int otherClientTick,
        String victimName
    ) {
        this.attackPosition = attackPosition;
        this.myClientTick = myClientTick;
        this.otherClientTick = otherClientTick;
        this.victimName = victimName;
    }
}
