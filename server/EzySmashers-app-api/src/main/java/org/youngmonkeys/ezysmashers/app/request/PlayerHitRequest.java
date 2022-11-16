package org.youngmonkeys.ezysmashers.app.request;

import com.tvd12.ezyfox.binding.annotation.EzyObjectBinding;
import lombok.Data;

@Data
@EzyObjectBinding
@SuppressWarnings("MemberName")
public class PlayerHitRequest {
    private int m; // myClientTick
    private int o; // otherClientTick
    private String v; // victimName
    private float[] p; // attackPosition
}
