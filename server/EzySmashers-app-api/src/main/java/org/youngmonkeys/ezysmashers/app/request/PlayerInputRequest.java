package org.youngmonkeys.ezysmashers.app.request;

import com.tvd12.ezyfox.binding.annotation.EzyObjectBinding;
import lombok.Data;

@Data
@EzyObjectBinding
@SuppressWarnings("MemberName")
public class PlayerInputRequest {
    private int t;
    private boolean[] k;
    private float[] r;
}
