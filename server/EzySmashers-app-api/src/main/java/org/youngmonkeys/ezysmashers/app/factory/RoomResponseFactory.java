package org.youngmonkeys.ezysmashers.app.factory;

import com.tvd12.ezyfox.bean.annotation.EzySingleton;
import com.tvd12.ezyfoxserver.support.command.EzyObjectResponse;
import com.tvd12.ezyfoxserver.support.factory.EzyResponseFactory;
import com.tvd12.gamebox.entity.MMORoom;
import lombok.AllArgsConstructor;
import org.youngmonkeys.ezysmashers.app.service.RoomService;

import java.util.List;

@EzySingleton
@AllArgsConstructor
public class RoomResponseFactory {

    private final RoomService roomService;
    private final EzyResponseFactory responseFactory;
    
    public EzyObjectResponse newSameRoomPlayersResponse(String playerName) {
        MMORoom currentRoom = (MMORoom) roomService.getCurrentRoom(playerName);
        List<String> playerNames = roomService.getRoomPlayerNames(currentRoom);
        
        return responseFactory.newObjectResponse()
            .usernames(playerNames);
    }
}
