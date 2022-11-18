package org.youngmonkeys.ezysmashers.app.controller;


import com.tvd12.ezyfox.core.annotation.EzyDoHandle;
import com.tvd12.ezyfox.core.annotation.EzyRequestController;
import com.tvd12.ezyfox.util.EzyLoggable;
import com.tvd12.ezyfoxserver.entity.EzyUser;
import com.tvd12.ezyfoxserver.support.factory.EzyResponseFactory;
import com.tvd12.gamebox.entity.MMOPlayer;
import lombok.AllArgsConstructor;
import org.youngmonkeys.ezysmashers.app.constant.Commands;
import org.youngmonkeys.ezysmashers.app.converter.ModelToResponseConverter;
import org.youngmonkeys.ezysmashers.app.converter.RequestToModelConverter;
import org.youngmonkeys.ezysmashers.app.model.AuthorizeHitModel;
import org.youngmonkeys.ezysmashers.app.request.PlayerHitRequest;
import org.youngmonkeys.ezysmashers.app.request.PlayerInputRequest;
import org.youngmonkeys.ezysmashers.app.service.GamePlayService;
import org.youngmonkeys.ezysmashers.app.service.RoomService;

import java.util.ArrayList;
import java.util.List;

import static org.youngmonkeys.ezysmashers.app.constant.PlayerHitConstants.FIELD_ATTACKER_NAME;

@AllArgsConstructor
@EzyRequestController
public class PlayerController extends EzyLoggable {

    private final RoomService roomService;
    private final GamePlayService gamePlayService;
    private final EzyResponseFactory responseFactory;
    private final RequestToModelConverter requestToModelConverter;
    private final ModelToResponseConverter modelToResponseConverter;

    @EzyDoHandle(Commands.PLAYER_INPUT)
    public void playerInput(EzyUser user, PlayerInputRequest request) {
        logger.info("user {} send input data {}", user.getName(), request);
        gamePlayService.handlePlayerInput(
            user.getName(),
            requestToModelConverter.toModel(request)
        );
    }

    @EzyDoHandle(Commands.PLAYER_HIT)
    public void playerHit(EzyUser user, PlayerHitRequest request) {
        logger.info("user {} send hit command {}", user.getName(), request);
        
        AuthorizeHitModel authorizePlayerHit = gamePlayService.authorizePlayerHit(
            user.getName(),
            requestToModelConverter.toModel(request)
        );

        modelToResponseConverter.toResponse(authorizePlayerHit)
            .command(Commands.PLAYER_BEING_ATTACKED)
            .execute();
    }

    @EzyDoHandle(Commands.PLAYER_ATTACK)
    public void playerAttack(EzyUser user) {
        logger.info("user {} send attack command", user.getName());

        MMOPlayer player = roomService.getPlayer(user.getName());
        List<String> nearbyPlayerNames = new ArrayList<>();
        player.getNearbyPlayerNames(nearbyPlayerNames);

        responseFactory.newObjectResponse()
            .command(Commands.PLAYER_ATTACK)
            .param(FIELD_ATTACKER_NAME, user.getName())
            .usernames(nearbyPlayerNames)
            .execute();
    }
}
