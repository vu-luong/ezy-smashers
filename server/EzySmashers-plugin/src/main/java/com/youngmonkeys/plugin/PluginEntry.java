/**
 * 
 */
package com.youngmonkeys.plugin;

import java.util.Properties;

import com.tvd12.ezyfox.bean.EzyBeanContextBuilder;
import com.tvd12.ezyfoxserver.context.EzyPluginContext;
import com.tvd12.ezyfoxserver.context.EzyZoneContext;
import com.tvd12.ezyfoxserver.setting.EzyPluginSetting;
import com.tvd12.ezyfoxserver.support.entry.EzyDefaultPluginEntry;

import com.youngmonkeys.common.constant.CommonConstants;

/**
 * @author tavandung12
 *
 */
public class PluginEntry extends EzyDefaultPluginEntry {

	@Override
	protected void preConfig(EzyPluginContext ctx) {
		logger.info("\n=================== EzySmashers PLUGIN START CONFIG ================\n");
	}
	
	@Override
	protected void postConfig(EzyPluginContext ctx) {
		logger.info("\n=================== EzySmashers PLUGIN END CONFIG ================\n");
	}
	
	@Override
	protected void setupBeanContext(EzyPluginContext context, EzyBeanContextBuilder builder) {
		EzyPluginSetting setting = context.getPlugin().getSetting();
		builder.addProperties("EzySmashers-common-config.properties");
		builder.addProperties(getConfigFile(setting));
		// this function will be removed when lucky-wheel update ezyfox-server to 1.2.0
		builder.scan(getScanablePackages());
		Properties properties = builder.getProperties();
		EzyZoneContext zoneContext = context.getParent();
		zoneContext.setProperty(CommonConstants.PLUGIN_PROPERTIES, properties);
	}

	protected String getConfigFile(EzyPluginSetting setting) {
		return setting.getConfigFile();
	}
	
	@Override
	protected String[] getScanablePackages() {
		return new String[] {
			"com.youngmonkeys.common",
			"com.youngmonkeys.plugin"
		};
	}
}