package org.youngmonkeys.ezysmashers.app.game;

import com.tvd12.ezyfox.annotation.EzyProperty;
import com.tvd12.ezyfox.bean.annotation.EzyAutoBind;
import com.tvd12.ezyfox.bean.annotation.EzySingleton;
import com.tvd12.gamebox.entity.MMORoom;
import com.tvd12.gamebox.handler.MMORoomUpdatedHandler;
import lombok.Setter;

@Setter
@EzySingleton
public class MMORoomFactory {

    @EzyProperty("game.room.max_players")
    private int maxPlayers;
    
    @EzyProperty("game.room.distance_of_interest")
    private double distanceOfInterest;

    @EzyAutoBind
    private MMORoomUpdatedHandler roomUpdatedHandler;
    
    public MMORoom newMMORoom() {
        return MMORoom.builder()
                .defaultPlayerManager(maxPlayers)
                .distanceOfInterest(distanceOfInterest)
                .addRoomUpdatedHandler(roomUpdatedHandler)
                .build();
    }
}
