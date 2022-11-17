package org.youngmonkeys.ezysmashers.app.converter;

import com.tvd12.ezyfox.bean.annotation.EzySingleton;
import lombok.AllArgsConstructor;
import org.youngmonkeys.ezysmashers.app.model.PlayerSpawnModel;
import org.youngmonkeys.ezysmashers.app.response.PlayerSpawnResponse;

@EzySingleton
@AllArgsConstructor
public class ModelToResponseConverter {

    public PlayerSpawnResponse toResponse(PlayerSpawnModel model) {
        return PlayerSpawnResponse.builder()
            .playerName(model.getPlayerName())
            .position(model.getPosition())
            .color(model.getColor())
            .build();
    }
}
