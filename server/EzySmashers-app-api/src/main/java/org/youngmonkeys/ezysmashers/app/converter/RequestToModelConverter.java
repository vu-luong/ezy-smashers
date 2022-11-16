package org.youngmonkeys.ezysmashers.app.converter;

import com.tvd12.ezyfox.bean.annotation.EzySingleton;
import lombok.AllArgsConstructor;
import org.youngmonkeys.ezysmashers.app.model.PlayerHitModel;
import org.youngmonkeys.ezysmashers.app.request.PlayerHitRequest;

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
}
