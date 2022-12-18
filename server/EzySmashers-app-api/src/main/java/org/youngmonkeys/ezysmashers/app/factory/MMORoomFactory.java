package org.youngmonkeys.ezysmashers.app.factory;

import com.tvd12.ezyfox.annotation.EzyProperty;
import com.tvd12.ezyfox.bean.annotation.EzyAutoBind;
import com.tvd12.ezyfox.bean.annotation.EzySingleton;
import com.tvd12.gamebox.entity.MMOGridRoom;
import com.tvd12.gamebox.entity.MMORoom;
import com.tvd12.gamebox.handler.MMORoomUpdatedHandler;
import lombok.Setter;

import static org.youngmonkeys.ezysmashers.app.constant.GameConstants.*;

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
        return MMOGridRoom.builder()
            .maxX(MAP_MAX_X)
            .maxY(MAP_MAX_Y)
            .maxZ(MAP_MAX_Z)
            .cellSize(MAP_CELL_SIZE)
            .maxPlayer(maxPlayers)
            .distanceOfInterest(distanceOfInterest)
            .addRoomUpdatedHandler(roomUpdatedHandler)
            .build();
    }
}
