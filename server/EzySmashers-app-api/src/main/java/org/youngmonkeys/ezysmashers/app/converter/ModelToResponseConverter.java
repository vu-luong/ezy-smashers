package org.youngmonkeys.ezysmashers.app.converter;

import com.tvd12.ezyfox.bean.annotation.EzySingleton;
import com.tvd12.ezyfoxserver.support.command.EzyObjectResponse;
import lombok.AllArgsConstructor;
import org.youngmonkeys.ezysmashers.app.factory.PlayerResponseFactory;
import org.youngmonkeys.ezysmashers.app.model.AuthorizeHitModel;
import org.youngmonkeys.ezysmashers.app.model.PlayerSpawnModel;
import org.youngmonkeys.ezysmashers.app.response.PlayerSpawnResponse;

import static org.youngmonkeys.ezysmashers.app.constant.PlayerHitConstants.*;

@EzySingleton
@AllArgsConstructor
public class ModelToResponseConverter {
    
    private final PlayerResponseFactory playerResponseFactory;

    public PlayerSpawnResponse toResponse(PlayerSpawnModel model) {
        return PlayerSpawnResponse.builder()
            .playerName(model.getPlayerName())
            .position(model.getPosition())
            .color(model.getColor())
            .build();
    }
    
    public EzyObjectResponse toResponse(AuthorizeHitModel model) {
        return playerResponseFactory.newObjectResponse(model.getPlayerName())
            .param(FIELD_ATTACKER_NAME, model.getPlayerName())
            .param(FIELD_ATTACK_TIME, model.getPlayerHit().getMyClientTick())
            .param(FIELD_ATTACK_POSITION, model.getPlayerHit().getAttackPosition())
            .param(FIELD_VICTIM_NAME, model.getPlayerHit().getVictimName());
    }
}
