package com.vuluong.plugin.controller;

import static com.tvd12.ezyfoxserver.constant.EzyEventNames.USER_LOGIN;

import com.vuluong.plugin.service.WelcomeService;
import com.tvd12.ezyfox.bean.annotation.EzyAutoBind;
import com.tvd12.ezyfox.bean.annotation.EzySingleton;
import com.tvd12.ezyfox.core.annotation.EzyEventHandler;
import com.tvd12.ezyfoxserver.context.EzyPluginContext;
import com.tvd12.ezyfoxserver.controller.EzyAbstractPluginEventController;
import com.tvd12.ezyfoxserver.event.EzyUserLoginEvent;

@EzySingleton
@EzyEventHandler(USER_LOGIN)
public class UserLoginController extends EzyAbstractPluginEventController<EzyUserLoginEvent> {

	@EzyAutoBind
	private WelcomeService welcomeService;
	
	@Override
	public void handle(EzyPluginContext ctx, EzyUserLoginEvent event) {
		logger.info("{} login in", welcomeService.welcome(event.getUsername()));
	}
	
}