package org.youngmonkeys.ezysmashers.app.service;

import com.tvd12.ezyfox.bean.annotation.EzySingleton;
import com.tvd12.ezyfox.util.EzyLoggable;
import com.tvd12.ezyfoxserver.entity.EzyUser;
import com.tvd12.gamebox.constant.RoomStatus;
import com.tvd12.gamebox.entity.*;
import com.tvd12.gamebox.manager.PlayerManager;
import com.tvd12.gamebox.manager.RoomManager;
import com.tvd12.gamebox.math.Vec3;
import lombok.AllArgsConstructor;
import org.youngmonkeys.ezysmashers.app.exception.CreateRoomWhenNotInLobbyException;
import org.youngmonkeys.ezysmashers.app.exception.JoinNonWaitingRoomException;
import org.youngmonkeys.ezysmashers.app.factory.MMORoomFactory;
import org.youngmonkeys.ezysmashers.app.model.JoinMMORoomModel;
import org.youngmonkeys.ezysmashers.app.model.JoinedMMORoomModel;
import org.youngmonkeys.ezysmashers.app.model.MMORoomPlayerNamesModel;

import java.util.List;
import java.util.stream.Collectors;

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
    
    public NormalRoom getCurrentRoom(Player player) {
        long currentRoomId = player.getCurrentRoomId();
        return globalRoomManager.getRoom(currentRoomId);
    }

    public NormalRoom getCurrentRoom(String playerName) {
        Player player = globalPlayerManager.getPlayer(playerName);
        return getCurrentRoom(player);
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

    public JoinedMMORoomModel playerJoinMMORoom(JoinMMORoomModel model) {
        Player player = globalPlayerManager.getPlayer(model.getPlayerName());
        lobbyRoom.removePlayer(player);
        MMORoom room = (MMORoom) globalRoomManager.getRoom(model.getRoomId());
        if (room.getStatus() != RoomStatus.WAITING) {
            throw new JoinNonWaitingRoomException(player.getName(), room);
        }
        synchronized (room) {
            room.addPlayer(player);
            player.setCurrentRoomId(room.getId());
        }
        return JoinedMMORoomModel.builder()
            .roomId(room.getId())
            .playerNames(getRoomPlayerNames(room))
            .build();
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

    public MMORoomPlayerNamesModel getMMORoomPlayerNames(String playerName) {
        MMORoom currentRoom = (MMORoom) getCurrentRoom(playerName);
        List<String> playerNames = getRoomPlayerNames(currentRoom);
        Player master = getMaster(currentRoom);
        return MMORoomPlayerNamesModel.builder()
            .playerNames(playerNames)
            .masterPlayerName(master.getName())
            .build();
    }
    
    public void setPlayerPosition(MMOPlayer player, Vec3 nextPosition) {
        MMOGridRoom currentRoom = (MMOGridRoom) getCurrentRoom(player);
        currentRoom.setPlayerPosition(player, nextPosition);
    }
}
