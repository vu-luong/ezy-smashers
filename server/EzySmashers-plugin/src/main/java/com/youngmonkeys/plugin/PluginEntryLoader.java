package com.youngmonkeys.plugin;

import com.tvd12.ezyfoxserver.ext.EzyAbstractPluginEntryLoader;
import com.tvd12.ezyfoxserver.ext.EzyPluginEntry;

public class PluginEntryLoader extends EzyAbstractPluginEntryLoader {

	@Override
	public EzyPluginEntry load() throws Exception {
		return new PluginEntry();
	}
	
}
