package org.youngmonkeys.ezysmashers.app.model;

import com.tvd12.ezyfox.entity.EzyArray;
import lombok.Builder;
import lombok.Getter;

@Getter
@Builder
public class PlayerSpawnModel {
    private String playerName;
    private EzyArray position;
    private EzyArray color;
}
