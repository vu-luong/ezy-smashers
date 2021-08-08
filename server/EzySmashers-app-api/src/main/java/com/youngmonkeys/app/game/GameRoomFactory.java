package com.youngmonkeys.app.game;

import com.tvd12.ezyfox.annotation.EzyProperty;
import com.tvd12.ezyfox.bean.annotation.EzyAutoBind;
import com.tvd12.ezyfox.bean.annotation.EzySingleton;
import com.tvd12.gamebox.entity.MMORoom;
import com.tvd12.gamebox.handler.MMORoomUpdatedHandler;

@EzySingleton
public class GameRoomFactory {

    @EzyProperty("game.room.max_players")
    private int maxPlayer;

    @EzyAutoBind
    private MMORoomUpdatedHandler roomUpdatedHandler;

    public GameRoom newGameRoom() {
        return new GameRoom(
            MMORoom.builder()
                .defaultPlayerManager(maxPlayer)
                .addRoomUpdatedHandler(roomUpdatedHandler)
                .build()
        );
    }
}
