package com.youngmonkeys.app.game;

import com.tvd12.ezyfox.annotation.EzyProperty;
import com.tvd12.ezyfox.bean.annotation.EzyAutoBind;
import com.tvd12.ezyfox.bean.annotation.EzyPropertiesSources;
import com.tvd12.ezyfox.bean.annotation.EzySingleton;
import com.tvd12.gamebox.handler.MMORoomUpdatedHandler;
import com.youngmonkeys.app.config.AppConfig;

@EzySingleton
public class GameRoomFactory {

    @EzyProperty("game.room.max_players")
    private int maxPlayers;
    
    @EzyProperty("game.room.distance_of_interest")
    private double distanceOfInterest;

    @EzyAutoBind
    private MMORoomUpdatedHandler roomUpdatedHandler;
    
    @EzyAutoBind
    private AppConfig appConfig;

    public GameRoom newGameRoom() {
        return GameRoom.builder()
                .defaultPlayerManager(maxPlayers)
                .distanceOfInterest(distanceOfInterest)
                .addRoomUpdatedHandler(roomUpdatedHandler)
                .build();
    }
}
