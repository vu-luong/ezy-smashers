package org.youngmonkeys.ezysmashers.app.request;

import com.tvd12.ezyfox.binding.annotation.EzyObjectBinding;
import lombok.Data;

@Data
@EzyObjectBinding
public class PlayerHitDataRequest {
	int m;
	int o;
	String v;
	float[] p;
}
