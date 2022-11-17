package org.youngmonkeys.ezysmashers.app.response;

import com.tvd12.ezyfox.binding.annotation.EzyObjectBinding;
import com.tvd12.ezyfox.entity.EzyArray;
import lombok.Builder;
import lombok.Getter;

@Getter
@Builder
@EzyObjectBinding
public class PlayerSpawnResponse {
    private String playerName;
    private EzyArray position;
    private EzyArray color;
}
