package com.vuluong.app;

import java.util.Properties;

import com.tvd12.ezyfox.bean.EzyBeanContextBuilder;
import com.tvd12.ezyfoxserver.context.EzyAppContext;
import com.tvd12.ezyfoxserver.context.EzyZoneContext;
import com.tvd12.ezyfoxserver.setting.EzyAppSetting;
import com.tvd12.ezyfoxserver.support.entry.EzyDefaultAppEntry;

import com.vuluong.common.constant.CommonConstants;

public class AppEntry extends EzyDefaultAppEntry {

	@Override
	protected void preConfig(EzyAppContext ctx) {
		logger.info("\n=================== EzyTank APP START CONFIG ================\n");
	}
	
	@Override
	protected void postConfig(EzyAppContext ctx) {
		logger.info("\n=================== EzyTank APP END CONFIG ================\n");
	}
	
	@Override
	protected void setupBeanContext(EzyAppContext context, EzyBeanContextBuilder builder) {
		EzyZoneContext zoneContext = context.getParent();
		Properties pluginProperties = zoneContext.getProperty(CommonConstants.PLUGIN_PROPERTIES);
		EzyAppSetting setting = context.getApp().getSetting();
		builder.addProperties("EzyTank-common-config.properties");
		builder.addProperties(pluginProperties);
		builder.addProperties(getConfigFile(setting));
		Properties properties = builder.getProperties();
	}
	
	protected String getConfigFile(EzyAppSetting setting) {
		return setting.getConfigFile();
	}
	
	@Override
	protected String[] getScanablePackages() {
		return new String[] {
				"com.vuluong.common",
				"com.vuluong.app"
		};
	}
}
