package org.youngmonkeys.ezysmashers.app.converter;

import com.tvd12.ezyfox.bean.annotation.EzySingleton;
import com.tvd12.ezyfox.io.EzyLists;
import com.tvd12.ezyfoxserver.entity.EzyUser;
import com.tvd12.ezyfoxserver.support.command.EzyObjectResponse;
import com.tvd12.ezyfoxserver.support.factory.EzyResponseFactory;
import lombok.AllArgsConstructor;
import org.youngmonkeys.ezysmashers.app.factory.RoomResponseFactory;
import org.youngmonkeys.ezysmashers.app.model.AuthorizeHitModel;
import org.youngmonkeys.ezysmashers.app.model.JoinedMMORoomModel;
import org.youngmonkeys.ezysmashers.app.model.MMORoomPlayerNamesModel;
import org.youngmonkeys.ezysmashers.app.model.PlayerSpawnModel;
import org.youngmonkeys.ezysmashers.app.response.PlayerSpawnResponse;

import static org.youngmonkeys.ezysmashers.app.constant.PlayerHitConstants.*;

@EzySingleton
@AllArgsConstructor
public class ModelToResponseConverter {

    private final EzyResponseFactory responseFactory;
    private final RoomResponseFactory roomResponseFactory;

    public PlayerSpawnResponse toResponse(PlayerSpawnModel model) {
        return PlayerSpawnResponse.builder()
            .playerName(model.getPlayerName())
            .position(model.getPosition())
            .color(model.getColor())
            .build();
    }

    public EzyObjectResponse toResponse(AuthorizeHitModel model) {
        return roomResponseFactory.newSameRoomPlayersResponse(model.getPlayerName())
            .param(FIELD_ATTACKER_NAME, model.getPlayerName())
            .param(FIELD_ATTACK_TIME, model.getPlayerHit().getMyClientTick())
            .param(FIELD_ATTACK_POSITION, model.getPlayerHit().getAttackPosition())
            .param(FIELD_VICTIM_NAME, model.getPlayerHit().getVictimName());
    }

    public EzyObjectResponse toResponse(MMORoomPlayerNamesModel model) {
        return responseFactory.newObjectResponse()
            .param("players", model.getPlayerNames())
            .param("master", model.getMasterPlayerName());
    }

    public EzyObjectResponse toResponse(
        EzyUser user,
        JoinedMMORoomModel model
    ) {
        return responseFactory.newObjectResponse()
            .param("roomId", model.getRoomId())
            .user(user);
    }

    public EzyObjectResponse toResponse(
        String playerName,
        JoinedMMORoomModel model
    ) {
        return responseFactory.newObjectResponse()
            .param("playerName", playerName)
            .usernames(
                EzyLists.filter(
                    model.getPlayerNames(), it -> !it.equals(playerName)
                )
            );
    }
}
