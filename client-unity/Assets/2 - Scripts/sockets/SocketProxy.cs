using System;
using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client;
using com.tvd12.ezyfoxserver.client.constant;
using com.tvd12.ezyfoxserver.client.entity;
using com.tvd12.ezyfoxserver.client.handler;
using com.tvd12.ezyfoxserver.client.request;
using com.tvd12.ezyfoxserver.client.support;
using com.tvd12.ezyfoxserver.client.util;
using UnityEngine;
using Object = System.Object;

#region App Data Handler

class CreateRoomResponseHandler : EzyAbstractAppDataHandler<EzyObject>
{
	public static event Action<int> action;
	protected override void process(EzyApp app, EzyObject data)
	{
		logger.info("Room created successfully: " + data.ToString());
		action?.Invoke(data.get<int>("roomId"));
	}
}

class GetMMORoomIdListResponseHandler : EzyAbstractAppDataHandler<EzyArray>
{
	public static event Action<List<int>> action;
	protected override void process(EzyApp app, EzyArray data)
	{
		logger.info("Room id list: " + data.get<EzyArray>(0).ToString());
		// TODO: should change to toList<long>() in the next version
		List<int> roomIdList = data.get<EzyArray>(0).toList<int>();
		action?.Invoke(roomIdList);
	}
}

class GetMMORoomPlayersResponseHandler : EzyAbstractAppDataHandler<EzyObject>
{
	public static event Action<List<string>, string> action;
	protected override void process(EzyApp app, EzyObject data)
	{
		logger.info("Current room's players: " + data);
		List<string> playerNames = data.get<EzyArray>("players").toList<string>();
		string masterName = data.get<string>("master");
		logger.info("Player Names: " + string.Join(",", playerNames));
		logger.info("Master Name: " + masterName);

		action?.Invoke(playerNames, masterName);
	}
}

class JoinMMORoomResponseHandler : EzyAbstractAppDataHandler<EzyObject>
{
	public static event Action<int> action;
	protected override void process(EzyApp app, EzyObject data)
	{
		logger.info("room id: " + data);
		int roomId = data.get<int>("roomId");
		action?.Invoke(roomId);
	}
}

class AnotherJoinMMORoomHandler : EzyAbstractAppDataHandler<EzyObject>
{
	public static event Action<string> action;
	protected override void process(EzyApp app, EzyObject data)
	{
		logger.info("Another player join room: " + data);
		string anotherName = data.get<string>("playerName");
		action?.Invoke(anotherName);
	}
}

class AnotherExitMMORoomHandler : EzyAbstractAppDataHandler<EzyObject>
{
	public static event Action<string> action;
	protected override void process(EzyApp app, EzyObject data)
	{
		logger.info("Another player exit room: " + data);
		string anotherName = data.get<string>("playerName");
		action?.Invoke(anotherName);
	}
}

class StartGameResponseHandler : EzyAbstractAppDataHandler<EzyArray>
{
	public static event Action<List<PlayerSpawnData>> action;
	protected override void process(EzyApp app, EzyArray data)
	{
		logger.info("Game start response" + data);
		List<PlayerSpawnData> spawnData = new List<PlayerSpawnData>();

		for (int i = 0; i < data.size(); i++)
		{
			EzyObject item = data.get<EzyObject>(i);
			string playerName = item.get<string>("playerName");
			List<float> position = item.get<EzyArray>("position").toList<float>();
			List<float> color = item.get<EzyArray>("color").toList<float>();
			spawnData.Add(
				new PlayerSpawnData(
					playerName,
					new Vector3(position[0], position[1], position[2]),
					new Vector3(color[0], color[1], color[2])
				)
			);
		}
		action?.Invoke(spawnData);
	}
}

class SyncPositionHandler : EzyAbstractAppDataHandler<EzyArray>
{
	public static event Action<string, Vector3, Vector3, int> action;
	protected override void process(EzyApp app, EzyArray data)
	{
		logger.info("Sync position: " + data);
		string playerName = data.get<string>(0);
		EzyArray positionArray = data.get<EzyArray>(1);
		EzyArray rotationArray = data.get<EzyArray>(2);
		int time = data.get<int>(3);
		Vector3 position = new Vector3(
			positionArray.get<float>(0),
			positionArray.get<float>(1),
			positionArray.get<float>(2)
		);

		Vector3 rotation = new Vector3(
			rotationArray.get<float>(0),
			rotationArray.get<float>(1),
			rotationArray.get<float>(2)
		);

		action?.Invoke(playerName, position, rotation, time);
	}
}

class PlayerBeingAttackedHandler : EzyAbstractAppDataHandler<EzyObject>
{
	public static event Action<string, string> action;
	protected override void process(EzyApp app, EzyObject data)
	{
		logger.info("Being Attacked: " + data);
		var victimName = data.get<string>("v");
		var attackTime = data.get<float>("t");
		var attackerName = data.get<string>("a");
		var attackPosition = data.get<EzyArray>("p");
		logger.info("victimName: " + victimName);
		logger.info("Attacker: " + attackerName);
		action?.Invoke(victimName, attackerName);
	}
}

class PlayerAttackDataHandler : EzyAbstractAppDataHandler<EzyObject>
{
	public static event Action<string> action;
	protected override void process(EzyApp app, EzyObject data)
	{
		logger.info("Attack: " + data);
		var attackerName = data.get<string>("a");
		action?.Invoke(attackerName);
	}

}

#endregion

public class SocketProxy : EzyLoggable
{
	private static readonly SocketProxy INSTANCE = new SocketProxy();

	public const string ZONE_NAME = "EzySmashers";
	public const string APP_NAME = "EzySmashers";

	private UserAuthenticationModel userAuthenticationModelAuthenInfo = new UserAuthenticationModel("test", "test1234");
	private EzyUTClient client;
	private string host;
	private int port;
	private EzySocketProxy socketProxy;
	private EzyAppProxy appProxy;

	public EzyUTClient Client { get => client; }
	public UserAuthenticationModel UserAuthenticationModelAuthenInfo { get => userAuthenticationModelAuthenInfo; set => userAuthenticationModelAuthenInfo = value; }
	public EzyAppProxy AppProxy { get => appProxy; set => appProxy = value; }
	public EzySocketProxy Proxy { get => socketProxy; set => socketProxy = value; }

	public static SocketProxy getInstance()
	{
		return INSTANCE;
	}

	public EzyUTClient setup(string host, int port)
	{
		this.host = host;
		this.port = port;

		logger.debug("Set up socket client");

		SocketProxyManager socketProxyManager = SocketProxyManager.getInstance();
		socketProxyManager.setDefaultZoneName(ZONE_NAME);
		socketProxyManager.init();

		socketProxy = socketProxyManager
			.getDefaultSocketProxy()
			.setTransportType(EzyTransportType.UDP)
			.setHost(host)
			.setLoginUsername(userAuthenticationModelAuthenInfo.Username)
			.setLoginPassword(userAuthenticationModelAuthenInfo.Password)
			.setDefaultAppName(APP_NAME);

		socketProxy.onLoginSuccess<Object>(HandleLoginSuccess);
		socketProxy.onAppAccessed<Object>(HandleAppAccessed);
		appProxy = socketProxy.getDefaultAppProxy();
		// appProxy.on<Object>(Commands.JOIN_LOBBY, (proxy, data) => )
		
		// setup.addDataHandler(EzyCommand.HANDSHAKE, new HandshakeHandler());
		// setup.addDataHandler(EzyCommand.LOGIN, new LoginSuccessHandler());
		// setup.addDataHandler(EzyCommand.UDP_HANDSHAKE, new UdpHandshakeHandler());
		// setup.addDataHandler(EzyCommand.APP_ACCESS, new AppAccessHandler());

		// Set up EzySmashers app
		// var appSetup = setup.setupApp(APP_NAME);
		// appSetup.addDataHandler(Commands.JOIN_LOBBY, new JoinLobbyResponseHandler());
		// appSetup.addDataHandler(Commands.CREATE_MMO_ROOM, new CreateRoomResponseHandler());
		// appSetup.addDataHandler(Commands.GET_MMO_ROOM_ID_LIST, new GetMMORoomIdListResponseHandler());
		// appSetup.addDataHandler(Commands.GET_MMO_ROOM_PLAYERS, new GetMMORoomPlayersResponseHandler());
		// appSetup.addDataHandler(Commands.JOIN_MMO_ROOM, new JoinMMORoomResponseHandler());
		// appSetup.addDataHandler(Commands.ANOTHER_JOIN_MMO_ROOM, new AnotherJoinMMORoomHandler());
		// appSetup.addDataHandler(Commands.ANOTHER_EXIT_MMO_ROOM, new AnotherExitMMORoomHandler());
		// appSetup.addDataHandler(Commands.START_GAME, new StartGameResponseHandler());
		// appSetup.addDataHandler(Commands.SYNC_POSITION, new SyncPositionHandler());
		// appSetup.addDataHandler(Commands.PLAYER_BEING_ATTACKED, new PlayerBeingAttackedHandler());
		// appSetup.addDataHandler(Commands.PLAYER_ATTACK_DATA, new PlayerAttackDataHandler());

		// Init RoomManager
		RoomManager.getInstance();
		client = (EzyUTClient)socketProxy.getClient();

		return client;
	}

	private void HandleLoginSuccess(EzySocketProxy socketProxy, Object data)
	{
		logger.debug("Log in successfully");
		// connect to udp port
		socketProxy.getClient().udpConnect(2611);
	}

	private void HandleAppAccessed(EzyAppProxy appProxy, Object data)
	{
		logger.debug("App access successfully");
		SocketRequest.getInstance().SendJoinLobbyRequest();
	}

	public void login(string username, string password)
	{
		userAuthenticationModelAuthenInfo.Username = username;
		userAuthenticationModelAuthenInfo.Password = password;
		client.connect(host, port);
	}
}
