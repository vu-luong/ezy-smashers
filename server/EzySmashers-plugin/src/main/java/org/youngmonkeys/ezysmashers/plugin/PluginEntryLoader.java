package org.youngmonkeys.ezysmashers.plugin;

import com.tvd12.ezyfoxserver.ext.EzyAbstractPluginEntryLoader;
import com.tvd12.ezyfoxserver.ext.EzyPluginEntry;

public class PluginEntryLoader extends EzyAbstractPluginEntryLoader {

	@Override
	public EzyPluginEntry load() {
		return new PluginEntry();
	}
}
