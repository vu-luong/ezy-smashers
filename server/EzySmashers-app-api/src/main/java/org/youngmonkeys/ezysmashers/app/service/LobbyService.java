package org.youngmonkeys.ezysmashers.app.service;

import com.tvd12.ezyfox.bean.annotation.EzySingleton;
import com.tvd12.gamebox.entity.MMOPlayer;
import com.tvd12.gamebox.entity.NormalRoom;
import lombok.AllArgsConstructor;
import lombok.Setter;
import org.youngmonkeys.ezysmashers.app.exception.AlreadyJoinedRoomException;

import java.util.List;

@Setter
@AllArgsConstructor
@EzySingleton
public class LobbyService {

    private NormalRoom lobbyRoom;
    private RoomService roomService;

    public void addNewPlayer(String playerName) {
        MMOPlayer player = new MMOPlayer(playerName);
        synchronized (lobbyRoom) {
            if (roomService.contains(player)) {
                throw new AlreadyJoinedRoomException(playerName, lobbyRoom);
            }
            lobbyRoom.addPlayer(player);
            player.setCurrentRoomId(lobbyRoom.getId());
        }

        roomService.addPlayer(player);
    }

    public List<String> getPlayerNames() {
        synchronized (lobbyRoom) {
            return lobbyRoom.getPlayerManager().getPlayerNames();
        }
    }

    public long getRoomId() {
        synchronized (lobbyRoom) {
            return lobbyRoom.getId();
        }
    }
}
