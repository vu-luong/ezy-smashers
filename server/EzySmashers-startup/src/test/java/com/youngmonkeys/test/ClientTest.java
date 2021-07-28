package com.youngmonkeys.test;

import com.youngmonkeys.ApplicationStartup;
import com.youngmonkeys.app.constant.Commands;
import com.tvd12.ezyfox.core.constant.EzyResponseCommands;
import com.tvd12.ezyfox.entity.EzyArray;
import com.tvd12.ezyfox.entity.EzyData;
import com.tvd12.ezyfox.factory.EzyEntityFactory;
import com.tvd12.ezyfoxserver.client.EzyClient;
import com.tvd12.ezyfoxserver.client.EzyClients;
import com.tvd12.ezyfoxserver.client.EzyTcpClient;
import com.tvd12.ezyfoxserver.client.config.EzyClientConfig;
import com.tvd12.ezyfoxserver.client.constant.EzyCommand;
import com.tvd12.ezyfoxserver.client.entity.EzyApp;
import com.tvd12.ezyfoxserver.client.handler.EzyAppAccessHandler;
import com.tvd12.ezyfoxserver.client.handler.EzyHandshakeHandler;
import com.tvd12.ezyfoxserver.client.handler.EzyLoginSuccessHandler;
import com.tvd12.ezyfoxserver.client.request.EzyAppAccessRequest;
import com.tvd12.ezyfoxserver.client.request.EzyLoginRequest;
import com.tvd12.ezyfoxserver.client.request.EzyRequest;
import com.tvd12.ezyfoxserver.client.setup.EzyAppSetup;
import com.tvd12.ezyfoxserver.client.setup.EzySetup;
import com.tvd12.ezyfoxserver.client.socket.EzyMainEventsLoop;

public class ClientTest {

	public static void main(String[] args) {
		EzyClientConfig config = EzyClientConfig.builder()
				.zoneName(ApplicationStartup.ZONE_APP_NAME)
				.build();
		EzyClient client = new EzyTcpClient(config);
		setupClient(client);
		EzyClients.getInstance().addClient(client);
		client.connect("127.0.0.1", 3005);
		EzyMainEventsLoop mainEventsLoop = new EzyMainEventsLoop();
		mainEventsLoop.start(5);
	}
	
	public static void setupClient(EzyClient client) {
		EzySetup setup = client.setup();
		setup.addDataHandler(EzyCommand.HANDSHAKE, new EzyHandshakeHandler() {
			
			@Override
			protected EzyRequest getLoginRequest() {
				return new EzyLoginRequest(
					ApplicationStartup.ZONE_APP_NAME,
					"YoungMonkey",
					"YoungMonkey"
				);
			}
		});
		setup.addDataHandler(EzyCommand.LOGIN, new EzyLoginSuccessHandler() {
			@Override
			protected void handleLoginSuccess(EzyData responseData) {
				client.send(new EzyAppAccessRequest(ApplicationStartup.ZONE_APP_NAME));
			}
		});
		setup.addDataHandler(EzyCommand.APP_ACCESS, new EzyAppAccessHandler() {
			@Override
			protected void postHandle(EzyApp app, EzyArray data) {
				app.send(Commands.HELLO, EzyEntityFactory.EMPTY_OBJECT);
				app.send(Commands.HELLO, EzyEntityFactory.newObjectBuilder()
						.append("nickName", "Dzung")
						.build());
				app.send(Commands.GO, EzyEntityFactory.EMPTY_OBJECT);
				app.send(Commands.GO, EzyEntityFactory.newObjectBuilder()
						.append("nickName", "Dzung")
						.build());
			}
		});
		
		EzyAppSetup appSetup = setup.setupApp(ApplicationStartup.ZONE_APP_NAME);
		appSetup.addDataHandler(EzyResponseCommands.ERROR, (app, data) -> {
			System.out.println("error: " + data);
		});
		appSetup.addDataHandler(Commands.HELLO, (app, data) -> {
			System.out.println("hello: " + data);
		});
		appSetup.addDataHandler(Commands.GO, (app, data) -> {
			System.out.println("hello: " + data);
		});
	}
}
