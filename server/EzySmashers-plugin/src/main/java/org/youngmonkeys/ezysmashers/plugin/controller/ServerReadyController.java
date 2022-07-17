package org.youngmonkeys.ezysmashers.plugin.controller;

import static com.tvd12.ezyfoxserver.constant.EzyEventNames.SERVER_READY;

import com.tvd12.ezyfox.bean.annotation.EzySingleton;
import com.tvd12.ezyfox.core.annotation.EzyEventHandler;
import com.tvd12.ezyfoxserver.context.EzyPluginContext;
import com.tvd12.ezyfoxserver.controller.EzyAbstractPluginEventController;
import com.tvd12.ezyfoxserver.event.EzyServerReadyEvent;

@EzySingleton
@EzyEventHandler(SERVER_READY)
public class ServerReadyController 
		extends EzyAbstractPluginEventController<EzyServerReadyEvent> {

	@Override
	public void handle(EzyPluginContext ctx, EzyServerReadyEvent event) {
		logger.info("EzySmashers plugin: fire custom app ready");
	}
	
}