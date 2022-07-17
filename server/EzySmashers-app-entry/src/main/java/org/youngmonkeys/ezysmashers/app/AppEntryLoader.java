package org.youngmonkeys.ezysmashers.app;

import com.tvd12.ezyfox.reflect.EzyClasses;
import com.tvd12.ezyfoxserver.ext.EzyAbstractAppEntryLoader;
import com.tvd12.ezyfoxserver.ext.EzyAppEntry;

public class AppEntryLoader extends EzyAbstractAppEntryLoader {

	@Override
	public EzyAppEntry load() {
		return EzyClasses.newInstance(
			"org.youngmonkeys.ezysmashers.app.AppEntry"
		);
	}

}
