package org.youngmonkeys.ezysmashers.app.config;

import com.tvd12.ezyfox.bean.annotation.EzyAutoBind;
import com.tvd12.ezyfox.bean.annotation.EzyConfigurationBefore;
import com.tvd12.ezyfox.bean.annotation.EzySingleton;
import com.tvd12.ezyfox.util.EzyLoggable;
import com.tvd12.gamebox.entity.NormalRoom;
import com.tvd12.gamebox.entity.Player;
import com.tvd12.gamebox.manager.PlayerManager;
import com.tvd12.gamebox.manager.RoomManager;
import com.tvd12.gamebox.manager.SynchronizedPlayerManager;
import com.tvd12.gamebox.manager.SynchronizedRoomManager;
import lombok.Setter;

@Setter
@EzyConfigurationBefore(priority = 1)
public class GameConfig extends EzyLoggable {
	
	@EzyAutoBind
	private NormalRoom lobbyRoom;
	
	@EzySingleton("globalRoomManager")
	public RoomManager<NormalRoom> globalRoomManager() {
		RoomManager<NormalRoom> roomManager = new SynchronizedRoomManager<>();
		roomManager.addRoom(lobbyRoom);
		return roomManager;
	}
	
	@EzySingleton("globalPlayerManager")
	public PlayerManager<Player> globalPlayerManager() {
		return new SynchronizedPlayerManager<>();
	}
}
