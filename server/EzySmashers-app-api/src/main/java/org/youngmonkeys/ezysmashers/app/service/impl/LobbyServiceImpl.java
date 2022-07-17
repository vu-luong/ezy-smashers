package org.youngmonkeys.ezysmashers.app.service.impl;

import com.tvd12.ezyfox.bean.annotation.EzyAutoBind;
import com.tvd12.ezyfox.bean.annotation.EzySingleton;
import com.tvd12.gamebox.entity.MMOPlayer;
import com.tvd12.gamebox.entity.NormalRoom;
import org.youngmonkeys.ezysmashers.app.exception.AlreadyJoinedRoomException;
import org.youngmonkeys.ezysmashers.app.service.LobbyService;
import org.youngmonkeys.ezysmashers.app.service.RoomService;
import lombok.Setter;

import java.util.List;

@Setter
@EzySingleton
public class LobbyServiceImpl implements LobbyService {

	@EzyAutoBind
	private NormalRoom lobbyRoom;

	@EzyAutoBind
	private RoomService roomService;

	@Override
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

	@Override
	public List<String> getPlayerNames() {
		synchronized (lobbyRoom) {
			return lobbyRoom.getPlayerManager().getPlayerNames();
		}
	}

	@Override
	public long getRoomId() {
		synchronized (lobbyRoom) {
			return lobbyRoom.getId();
		}
	}
}
