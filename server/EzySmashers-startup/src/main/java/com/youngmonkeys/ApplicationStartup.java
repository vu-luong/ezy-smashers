package com.youngmonkeys;

import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;

import com.tvd12.ezyfoxserver.constant.EzyEventType;
import com.tvd12.ezyfoxserver.embedded.EzyEmbeddedServer;
import com.tvd12.ezyfoxserver.ext.EzyAppEntry;
import com.tvd12.ezyfoxserver.ext.EzyPluginEntry;
import com.tvd12.ezyfoxserver.setting.EzyAppSetting;
import com.tvd12.ezyfoxserver.setting.EzyAppSettingBuilder;
import com.tvd12.ezyfoxserver.setting.EzyPluginSetting;
import com.tvd12.ezyfoxserver.setting.EzyPluginSettingBuilder;
import com.tvd12.ezyfoxserver.setting.EzySettingsBuilder;
import com.tvd12.ezyfoxserver.setting.EzySimpleSettings;
import com.tvd12.ezyfoxserver.setting.EzyZoneSettingBuilder;
import com.youngmonkeys.plugin.PluginEntry;
import com.youngmonkeys.plugin.PluginEntryLoader;
import com.youngmonkeys.app.AppEntry;
import com.youngmonkeys.app.AppEntryLoader;


public class ApplicationStartup {
	
	public static final String ZONE_APP_NAME = "EzySmashers"; 
	
	public static void main(String[] args) throws Exception {
		
		EzyPluginSettingBuilder pluginSettingBuilder = new EzyPluginSettingBuilder()
				.name(ZONE_APP_NAME)
				.addListenEvent(EzyEventType.USER_LOGIN)
				.entryLoader(DecoratedPluginEntryLoader.class);
		
		EzyAppSettingBuilder appSettingBuilder = new EzyAppSettingBuilder()
				.name(ZONE_APP_NAME)
				.entryLoader(DecoratedAppEntryLoader.class);
		
		EzyZoneSettingBuilder zoneSettingBuilder = new EzyZoneSettingBuilder()
				.name(ZONE_APP_NAME)
				.application(appSettingBuilder.build())
				.plugin(pluginSettingBuilder.build());
		
		EzySimpleSettings settings = new EzySettingsBuilder()
				.zone(zoneSettingBuilder.build())
				.build();
		
		EzyEmbeddedServer server = EzyEmbeddedServer.builder()
				.settings(settings)
				.build();
		server.start();
		
	}
	
	public static class DecoratedPluginEntryLoader extends PluginEntryLoader {
		
		@Override
		public EzyPluginEntry load() throws Exception {
			return new PluginEntry() {
				
				@Override
				protected String getConfigFile(EzyPluginSetting setting) {
					return Paths.get(getPluginPath(setting), "config", "config.properties")
							.toString();
				}
				
				private String getPluginPath(EzyPluginSetting setting) {
					Path pluginPath = Paths.get("EzySmashers-plugin");
					if(!Files.exists(pluginPath))
						pluginPath = Paths.get("../EzySmashers-plugin");
					return pluginPath.toString();
				}
			};
		}
	}
	
	public static class DecoratedAppEntryLoader extends AppEntryLoader {
		
		@Override
		public EzyAppEntry load() throws Exception {
			return new AppEntry() {
				
				@Override
				protected String getConfigFile(EzyAppSetting setting) {
					return Paths.get(getAppPath(), "config", "config.properties")
							.toString();
				}
				
				private String getAppPath() {
					Path pluginPath = Paths.get("EzySmashers-app-entry");
					if(!Files.exists(pluginPath))
						pluginPath = Paths.get("../EzySmashers-app-entry");
					return pluginPath.toString();
				}
			};
		}
	}
}
