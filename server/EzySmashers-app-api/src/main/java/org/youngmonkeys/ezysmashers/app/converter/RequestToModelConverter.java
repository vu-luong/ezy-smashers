package org.youngmonkeys.ezysmashers.app.converter;

import com.tvd12.ezyfox.bean.annotation.EzySingleton;
import lombok.AllArgsConstructor;
import org.youngmonkeys.ezysmashers.app.model.JoinMMORoomModel;
import org.youngmonkeys.ezysmashers.app.model.PlayerHitModel;
import org.youngmonkeys.ezysmashers.app.model.PlayerInputModel;
import org.youngmonkeys.ezysmashers.app.request.PlayerHitRequest;
import org.youngmonkeys.ezysmashers.app.request.PlayerInputRequest;

@EzySingleton
@AllArgsConstructor
public class RequestToModelConverter {

    public PlayerHitModel toModel(PlayerHitRequest request) {
        return PlayerHitModel.builder()
            .attackPosition(request.getP())
            .myClientTick(request.getM())
            .otherClientTick(request.getO())
            .victimName(request.getV())
            .build();
    }

    public PlayerInputModel toModel(PlayerInputRequest request) {
        return PlayerInputModel.builder()
            .time(request.getT())
            .inputs(request.getK())
            .rotation(request.getR())
            .build();
    }

    public JoinMMORoomModel toModel(String playerName, long roomId) {
        return JoinMMORoomModel.builder()
            .playerName(playerName)
            .roomId(roomId)
            .build();
    }
}
