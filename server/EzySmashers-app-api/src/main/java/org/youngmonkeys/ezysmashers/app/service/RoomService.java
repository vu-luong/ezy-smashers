package org.youngmonkeys.ezysmashers.app.service;

import com.tvd12.ezyfoxserver.entity.EzyUser;
import com.tvd12.gamebox.entity.MMOPlayer;
import com.tvd12.gamebox.entity.MMORoom;
import com.tvd12.gamebox.entity.NormalRoom;
import com.tvd12.gamebox.entity.Player;

import java.util.List;

public interface RoomService {
	NormalRoom removePlayer(String username);

	void addPlayer(MMOPlayer player);

	MMORoom newMMORoom(EzyUser user);

	MMOPlayer getPlayer(String playerName);

	List<String> getRoomPlayerNames(NormalRoom room);

	List<Player> getRoomPlayers(NormalRoom room);

	void addRoom(NormalRoom room);

	List<Long> getMMORoomIdList();

	NormalRoom getCurrentRoom(String playerName);

	Player getMaster(NormalRoom currentRoom);

	MMORoom playerJoinMMORoom(String playerName, long roomId);

	void removePlayerFromGameRoom(String playerName, MMORoom room);

	boolean contains(MMOPlayer player);
}
