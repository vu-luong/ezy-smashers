package com.youngmonkeys.app.config;

import com.tvd12.ezyfox.bean.annotation.EzyConfigurationBefore;
import com.tvd12.ezyfox.bean.annotation.EzySingleton;
import com.tvd12.ezyfox.util.EzyLoggable;
import com.tvd12.gamebox.entity.MMOVirtualWorld;
import com.tvd12.gamebox.entity.NormalRoom;

@EzyConfigurationBefore
public class MMOVirtualWorldConfig extends EzyLoggable {
	
	@EzySingleton
	public MMOVirtualWorld mmoVirtualWorld() {
		logger.info("Initialize MMO Virtual World");
		return MMOVirtualWorld.builder().build();
	}
	
	@EzySingleton("lobbyRoom")
	public NormalRoom lobbyRoom() {
		logger.info("Initialize lobby room");
		return NormalRoom.builder()
				.build();
	}
}
