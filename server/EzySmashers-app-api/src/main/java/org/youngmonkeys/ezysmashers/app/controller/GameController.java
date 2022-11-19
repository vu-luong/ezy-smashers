package org.youngmonkeys.ezysmashers.app.controller;

import com.tvd12.ezyfox.core.annotation.EzyDoHandle;
import com.tvd12.ezyfox.core.annotation.EzyRequestController;
import com.tvd12.ezyfox.util.EzyLoggable;
import com.tvd12.ezyfoxserver.entity.EzyUser;
import com.tvd12.ezyfoxserver.support.factory.EzyResponseFactory;
import lombok.AllArgsConstructor;
import org.youngmonkeys.ezysmashers.app.constant.Commands;
import org.youngmonkeys.ezysmashers.app.converter.ModelToResponseConverter;
import org.youngmonkeys.ezysmashers.app.model.StartGameModel;
import org.youngmonkeys.ezysmashers.app.service.GamePlayService;

import static com.tvd12.ezyfox.io.EzyLists.newArrayList;

@AllArgsConstructor
@EzyRequestController
public class GameController extends EzyLoggable {

    private final GamePlayService gamePlayService;
    private final EzyResponseFactory responseFactory;
    private final ModelToResponseConverter modelToResponseConverter;

    @EzyDoHandle(Commands.START_GAME)
    public void startGame(EzyUser user) {
        logger.info("user {} start game", user);
        
        StartGameModel startGame = gamePlayService.startGame(user);

        responseFactory.newArrayResponse()
            .command(Commands.START_GAME)
            .data(
                newArrayList(
                    startGame.getPlayerSpawns(),
                    modelToResponseConverter::toResponse
                )
            )
            .usernames(startGame.getPlayerNames())
            .execute();
    }
}
