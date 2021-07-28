package com.youngmonkeys.plugin.service;

import com.youngmonkeys.common.service.CommonService;
import com.youngmonkeys.plugin.config.PluginConfig;
import com.tvd12.ezyfox.bean.annotation.EzyAutoBind;
import com.tvd12.ezyfox.bean.annotation.EzySingleton;

@EzySingleton
public class WelcomeService {

	@EzyAutoBind
	private PluginConfig config;
	
	@EzyAutoBind
	private CommonService commonService;
	
	public String welcome(String username) {
		return config.getWelcomePrefix() + " " + username + "!";
	}
	
}
