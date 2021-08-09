package com.youngmonkeys.app.game;

import com.tvd12.ezyfox.annotation.EzyProperty;
import com.tvd12.ezyfox.bean.annotation.EzyAutoBind;
import com.tvd12.ezyfox.bean.annotation.EzySingleton;
import com.tvd12.gamebox.handler.MMORoomUpdatedHandler;

@EzySingleton
public class GameRoomFactory {

    @EzyProperty("game.room.max_players")
    private int maxPlayer;
    
    @EzyProperty("game.room.distance_of_interest")
    private double distanceOfInterest;

    @EzyAutoBind
    private MMORoomUpdatedHandler roomUpdatedHandler;

    public GameRoom newGameRoom() {
        return (GameRoom) GameRoom.builder()
                .defaultPlayerManager(maxPlayer)
                .distanceOfInterest(distanceOfInterest)
                .addRoomUpdatedHandler(roomUpdatedHandler)
                .build();
    }
}
