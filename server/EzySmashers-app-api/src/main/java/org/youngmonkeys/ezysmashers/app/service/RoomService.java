package org.youngmonkeys.ezysmashers.app.service;

import com.tvd12.ezyfox.bean.annotation.EzySingleton;
import com.tvd12.ezyfox.util.EzyLoggable;
import com.tvd12.ezyfoxserver.entity.EzyUser;
import com.tvd12.gamebox.constant.RoomStatus;
import com.tvd12.gamebox.entity.*;
import com.tvd12.gamebox.manager.PlayerManager;
import com.tvd12.gamebox.manager.RoomManager;
import lombok.AllArgsConstructor;
import lombok.Setter;
import org.youngmonkeys.ezysmashers.app.exception.CreateRoomWhenNotInLobbyException;
import org.youngmonkeys.ezysmashers.app.factory.MMORoomFactory;

import java.util.List;
import java.util.stream.Collectors;

@Setter
@EzySingleton
@AllArgsConstructor
@SuppressWarnings({"unchecked"})
public class RoomService extends EzyLoggable {

    private MMOVirtualWorld mmoVirtualWorld;
    private MMORoomFactory gameRoomFactory;
    private PlayerManager<Player> globalPlayerManager;
    private RoomManager<NormalRoom> globalRoomManager;
    private NormalRoom lobbyRoom;

    public NormalRoom removePlayer(String username) {
        Player player = globalPlayerManager.getPlayer(username);
        if (player == null) {
            return null;
        }
        NormalRoom room = globalRoomManager.getRoom(player.getCurrentRoomId());
        if (room != null) {
            synchronized (room) {
                room.removePlayer(player);
                if (room.getPlayerManager().isEmpty() && (room.getId() != lobbyRoom.getId())) {
                    globalRoomManager.removeRoom(room);
                }
            }
        }

        globalPlayerManager.removePlayer(player);
        return room;
    }

    public void addPlayer(MMOPlayer player) {
        globalPlayerManager.addPlayer(player);
    }

    public MMORoom newMMORoom(EzyUser user) {
        MMOPlayer player = getPlayer(user.getName());
        if (player.getCurrentRoomId() != lobbyRoom.getId()) {
            throw new CreateRoomWhenNotInLobbyException(player.getName());
        }
        MMORoom room = gameRoomFactory.newMMORoom();
        room.setStatus(RoomStatus.WAITING);
        room.addPlayer(player);
        lobbyRoom.removePlayer(player);
        player.setCurrentRoomId(room.getId());

        this.addRoom(room);
        return room;
    }

    public MMOPlayer getPlayer(String playerName) {
        return (MMOPlayer) globalPlayerManager.getPlayer(playerName);
    }

    public List<String> getRoomPlayerNames(NormalRoom room) {
        synchronized (room) {
            return room.getPlayerManager().getPlayerNames();
        }
    }

    public List<Player> getRoomPlayers(NormalRoom room) {
        synchronized (room) {
            return room.getPlayerManager().getPlayerList();
        }
    }

    public void addRoom(NormalRoom room) {
        if (room instanceof MMORoom) {
            synchronized (mmoVirtualWorld) {
                mmoVirtualWorld.addRoom((MMORoom) room);
            }
        }
        globalRoomManager.addRoom(room);
    }

    public List<Long> getMMORoomIdList() {
        return globalRoomManager
            .getRoomList()
            .stream()
            .filter(room -> !room.getName().equals(lobbyRoom.getName()))
            .filter(room -> room.getStatus() == RoomStatus.WAITING)
            .map(Room::getId)
            .collect(Collectors.toList());
    }

    public NormalRoom getCurrentRoom(String playerName) {
        Player player = globalPlayerManager.getPlayer(playerName);
        long currentRoomId = player.getCurrentRoomId();
        return globalRoomManager.getRoom(currentRoomId);
    }

    public Player getMaster(NormalRoom room) {
        synchronized (room) {
            if (room instanceof MMORoom) {
                return ((MMORoom) room).getMaster();
            } else {
                return null;
            }
        }
    }

    /**
     * MMOPlayer join MMORoom.
     *
     * @param playerName
     *     name of player to join MMO room
     * @param roomId
     *     id of an MMORoom
     */
    public MMORoom playerJoinMMORoom(String playerName, long roomId) {
        Player player = globalPlayerManager.getPlayer(playerName);
        lobbyRoom.removePlayer(player);
        MMORoom room = (MMORoom) globalRoomManager.getRoom(roomId);

        synchronized (room) {
            room.addPlayer((MMOPlayer) player);
            player.setCurrentRoomId(room.getId());
        }

        return room;
    }

    public void removePlayerFromGameRoom(String playerName, MMORoom room) {
        MMOPlayer victim = getPlayer(playerName);
        room.removePlayer(victim); // synchronized already
        synchronized (victim) {
            lobbyRoom.addPlayer(victim);
            victim.setCurrentRoomId(lobbyRoom.getId());
        }
    }

    public boolean contains(MMOPlayer player) {
        return globalPlayerManager.containsPlayer(player);
    }
}
