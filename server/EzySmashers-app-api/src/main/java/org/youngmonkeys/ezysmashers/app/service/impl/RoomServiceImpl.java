package org.youngmonkeys.ezysmashers.app.service.impl;

import com.tvd12.ezyfox.bean.annotation.EzyAutoBind;
import com.tvd12.ezyfox.bean.annotation.EzySingleton;
import com.tvd12.ezyfox.util.EzyLoggable;
import com.tvd12.ezyfoxserver.entity.EzyUser;
import com.tvd12.gamebox.constant.RoomStatus;
import com.tvd12.gamebox.entity.*;
import com.tvd12.gamebox.manager.PlayerManager;
import com.tvd12.gamebox.manager.RoomManager;
import org.youngmonkeys.ezysmashers.app.exception.CreateRoomNotFromLobbyException;
import org.youngmonkeys.ezysmashers.app.game.MMORoomFactory;
import org.youngmonkeys.ezysmashers.app.service.RoomService;
import lombok.Setter;

import java.util.List;
import java.util.stream.Collectors;

@Setter
@EzySingleton
@SuppressWarnings({"unchecked"})
public class RoomServiceImpl extends EzyLoggable implements RoomService {

	@EzyAutoBind
	private MMOVirtualWorld mmoVirtualWorld;

	@EzyAutoBind
	private MMORoomFactory gameRoomFactory;

	@EzyAutoBind
	private PlayerManager<Player> globalPlayerManager;

	@EzyAutoBind
	private RoomManager<NormalRoom> globalRoomManager;

	@EzyAutoBind
	private NormalRoom lobbyRoom;

	@Override
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

	@Override
	public void addPlayer(MMOPlayer player) {
		globalPlayerManager.addPlayer(player);
	}

	@Override
	public MMORoom newMMORoom(EzyUser user) {
		MMOPlayer player = getPlayer(user.getName());
		if (player.getCurrentRoomId() != lobbyRoom.getId()) {
			throw new CreateRoomNotFromLobbyException(player.getName());
		}
		MMORoom room = gameRoomFactory.newMMORoom();
		room.setStatus(RoomStatus.WAITING);
		room.addPlayer(player);
		lobbyRoom.removePlayer(player);
		player.setCurrentRoomId(room.getId());

		this.addRoom(room);
		return room;
	}

	@Override
	public MMOPlayer getPlayer(String playerName) {
		return (MMOPlayer) globalPlayerManager.getPlayer(playerName);
	}

	@Override
	public List<String> getRoomPlayerNames(NormalRoom room) {
		synchronized (room) {
			return room.getPlayerManager().getPlayerNames();
		}
	}

	@Override
	public List<Player> getRoomPlayers(NormalRoom room) {
		synchronized (room) {
			return room.getPlayerManager().getPlayerList();
		}
	}

	@Override
	public void addRoom(NormalRoom room) {
		if (room instanceof MMORoom) {
			synchronized (mmoVirtualWorld) {
				mmoVirtualWorld.addRoom((MMORoom) room);
			}
		}
		globalRoomManager.addRoom(room);
	}

	@Override
	public List<Long> getMMORoomIdList() {
		return globalRoomManager
				.getRoomList()
				.stream()
				.filter(room -> !room.getName().equals(lobbyRoom.getName()))
				.filter(room -> room.getStatus() == RoomStatus.WAITING)
				.map(Room::getId)
				.collect(Collectors.toList());
	}

	@Override
	public NormalRoom getCurrentRoom(String playerName) {
		Player player = globalPlayerManager.getPlayer(playerName);
		long currentRoomId = player.getCurrentRoomId();
		return globalRoomManager.getRoom(currentRoomId);
	}

	@Override
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
	 * MMOPlayer join MMORoom
	 *
	 * @param playerName name of player to join MMO room
	 * @param roomId     id of an MMORoom
	 */
	@Override
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

	@Override
	public void removePlayerFromGameRoom(String playerName, MMORoom room) {
		MMOPlayer victim = getPlayer(playerName);
		room.removePlayer(victim); // synchronized already
		synchronized (victim) {
			lobbyRoom.addPlayer(victim);
			victim.setCurrentRoomId(lobbyRoom.getId());
		}
	}

	@Override
	public boolean contains(MMOPlayer player) {
		return globalPlayerManager.containsPlayer(player);
	}
}
