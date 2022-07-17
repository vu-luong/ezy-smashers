package org.youngmonkeys.ezysmashers.app.request;

import com.tvd12.ezyfox.binding.annotation.EzyObjectBinding;
import lombok.Data;

@Data
@EzyObjectBinding
public class JoinMMORoomRequest {

	private int roomId;
	
}
