package org.youngmonkeys.ezysmashers.app.service;

import com.tvd12.ezyfox.bean.annotation.EzySingleton;
import com.tvd12.gamebox.entity.MMOPlayer;
import lombok.AllArgsConstructor;

import java.util.ArrayList;
import java.util.List;

@EzySingleton
@AllArgsConstructor
public class PlayerService {
    
    private final RoomService roomService;
    
    public List<String> getNearbyPlayerNames(String playerName) {
        MMOPlayer player = roomService.getPlayer(playerName);
        List<String> nearbyPlayerNames = new ArrayList<>();
        player.getNearbyPlayerNames(nearbyPlayerNames);
        return nearbyPlayerNames;
    }
}
