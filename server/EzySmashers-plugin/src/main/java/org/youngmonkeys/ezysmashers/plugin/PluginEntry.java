package org.youngmonkeys.ezysmashers.plugin;

import com.tvd12.ezyfoxserver.context.EzyPluginContext;
import com.tvd12.ezyfoxserver.support.entry.EzyDefaultPluginEntry;

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
	protected String[] getScanablePackages() {
		return new String[] {
			"org.youngmonkeys.ezysmashers.common",
			"org.youngmonkeys.ezysmashers.plugin"
		};
	}
}
