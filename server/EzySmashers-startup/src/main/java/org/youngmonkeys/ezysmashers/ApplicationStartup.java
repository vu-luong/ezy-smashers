package org.youngmonkeys.ezysmashers;

import com.tvd12.ezyfoxserver.constant.EzyEventType;
import com.tvd12.ezyfoxserver.embedded.EzyEmbeddedServer;
import com.tvd12.ezyfoxserver.ext.EzyAppEntry;
import com.tvd12.ezyfoxserver.setting.*;
import org.youngmonkeys.ezysmashers.app.AppEntry;
import org.youngmonkeys.ezysmashers.app.AppEntryLoader;
import org.youngmonkeys.ezysmashers.plugin.PluginEntryLoader;

import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;

public class ApplicationStartup {
	
	public static final String ZONE_APP_NAME = "EzySmashers";
	
	public static void main(String[] args) throws Exception {
		
		EzyPluginSettingBuilder pluginSettingBuilder = new EzyPluginSettingBuilder()
				.name(ZONE_APP_NAME)
				.addListenEvent(EzyEventType.USER_LOGIN)
				.entryLoader(PluginEntryLoader.class);
		
		EzyAppSettingBuilder appSettingBuilder = new EzyAppSettingBuilder()
				.name(ZONE_APP_NAME)
				.entryLoader(DecoratedAppEntryLoader.class);
		
		EzyZoneSettingBuilder zoneSettingBuilder = new EzyZoneSettingBuilder()
				.name(ZONE_APP_NAME)
				.application(appSettingBuilder.build())
				.plugin(pluginSettingBuilder.build());
		
		EzyWebSocketSettingBuilder webSocketSettingBuilder = new EzyWebSocketSettingBuilder()
				.active(true);
		
		EzyUdpSettingBuilder udpSettingBuilder = new EzyUdpSettingBuilder()
				.active(true);
		
		EzySessionManagementSettingBuilder sessionManagementSettingBuilder = new EzySessionManagementSettingBuilder()
				.sessionMaxRequestPerSecond(
						new EzySessionManagementSettingBuilder.EzyMaxRequestPerSecondBuilder()
								.value(250)
								.build()
				);
		
		EzySimpleSettings settings = new EzySettingsBuilder()
				.zone(zoneSettingBuilder.build())
				.websocket(webSocketSettingBuilder.build())
				.udp(udpSettingBuilder.build())
				.sessionManagement(sessionManagementSettingBuilder.build())
				.build();
		
		EzyEmbeddedServer server = EzyEmbeddedServer.builder()
				.settings(settings)
				.build();
		server.start();
		
	}
	
	public static class DecoratedAppEntryLoader extends AppEntryLoader {
		
		@Override
		public EzyAppEntry load() {
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
