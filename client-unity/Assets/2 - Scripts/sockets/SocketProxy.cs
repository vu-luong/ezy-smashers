using System;
using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client;
using com.tvd12.ezyfoxserver.client.config;
using com.tvd12.ezyfoxserver.client.constant;
using com.tvd12.ezyfoxserver.client.entity;
using com.tvd12.ezyfoxserver.client.handler;
using com.tvd12.ezyfoxserver.client.request;
using com.tvd12.ezyfoxserver.client.util;
using UnityEngine;

class HandshakeHandler : EzyHandshakeHandler
{
	protected override EzyRequest getLoginRequest()
	{
		return new EzyLoginRequest(
			SocketProxy.ZONE_NAME,
			SocketProxy.getInstance().UserAuthenInfo.Username,
			SocketProxy.getInstance().UserAuthenInfo.Password
		);
	}
}

class LoginSuccessHandler : EzyLoginSuccessHandler
{
	protected override void handleLoginSuccess(EzyData responseData)
	{
		logger.debug("Log in successfully");
		// connect to udp port
		client.udpConnect(2611);
	}
}

class UdpHandshakeHandler : EzyUdpHandshakeHandler
{
	protected override void onAuthenticated(EzyArray data)
	{
		logger.debug("UdpHandshakeHandler authenticated");
		SocketRequest.getInstance().SendAppAccessRequest();
	}
}

class AppAccessHandler : EzyAppAccessHandler
{
	protected override void postHandle(EzyApp app, EzyArray data)
	{
		logger.debug("App access successfully");
		SocketRequest.getInstance().SendJoinLobbyRequest();
	}
}

#region App Data Handler

class JoinLobbyResponseHandler : EzyAbstractAppDataHandler<EzyObject>
{
	public static event Action joinedLobbyEvent;
	protected override void process(EzyApp app, EzyObject data)
	{
		joinedLobbyEvent?.Invoke();
	}
}

class CreateRoomResponseHandler : EzyAbstractAppDataHandler<EzyObject>
{
	public static event Action<int> roomCreatedEvent;
	protected override void process(EzyApp app, EzyObject data)
	{
		logger.info("Room created successfully: " + data.ToString());
		roomCreatedEvent?.Invoke(data.get<int>("roomId"));
	}
}

class GetMMORoomIdListResponse : EzyAbstractAppDataHandler<EzyArray>
{
	public static event Action<List<int>> mmoRoomIdListResponseEvent;
	protected override void process(EzyApp app, EzyArray data)
	{
		logger.info("Room id list: " + data.get<EzyArray>(0).ToString());
		// TODO: should change to toList<long>() in the next version
		List<int> roomIdList = data.get<EzyArray>(0).toList<int>();
		mmoRoomIdListResponseEvent?.Invoke(roomIdList);
	}
}

class GetMMORoomPlayersResponseHandler : EzyAbstractAppDataHandler<EzyObject>
{
	public static event Action<List<string>, string> mmoRoomPlayersResponseEvent;
	protected override void process(EzyApp app, EzyObject data)
	{
		logger.info("Current room's players: " + data);
		List<string> playerNames = data.get<EzyArray>("players").toList<string>();
		string masterName = data.get<string>("master");
		logger.info("Player Names: " + string.Join(",", playerNames));
		logger.info("Master Name: " + masterName);

		mmoRoomPlayersResponseEvent?.Invoke(playerNames, masterName);
	}
}

class JoinMMORoomResponseHandler : EzyAbstractAppDataHandler<EzyObject>
{
	public static event Action<int> joinRoomResponseEvent;
	protected override void process(EzyApp app, EzyObject data)
	{
		logger.info("room id: " + data);
		int roomId = data.get<int>("roomId");
		joinRoomResponseEvent?.Invoke(roomId);
	}
}

class AnotherJoinMMORoomHandler : EzyAbstractAppDataHandler<EzyObject>
{
	public static event Action<string> anotherJoinMMORoomEvent;
	protected override void process(EzyApp app, EzyObject data)
	{
		logger.info("Another player join room: " + data);
		string anotherName = data.get<string>("playerName");
		anotherJoinMMORoomEvent?.Invoke(anotherName);
	}
}

class AnotherExitMMORoomHandler : EzyAbstractAppDataHandler<EzyObject>
{
	public static event Action<string> anotherExitMMORoomEvent;
	protected override void process(EzyApp app, EzyObject data)
	{
		logger.info("Another player exit room: " + data);
		string anotherName = data.get<string>("playerName");
		anotherExitMMORoomEvent?.Invoke(anotherName);
	}
}

class StartGameResponseHandler : EzyAbstractAppDataHandler<EzyArray>
{
	public static event Action<List<PlayerSpawnData>> startGameResponseEvent;
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
			spawnData.Add(new PlayerSpawnData(
				              playerName,
				              new Vector3(position[0], position[1], position[2]),
				              new Vector3(color[0], color[1], color[2])
			              )
			);
		}
		startGameResponseEvent?.Invoke(spawnData);
	}
}

class SyncPositionHandler : EzyAbstractAppDataHandler<EzyArray>
{
	public static event Action<string, Vector3, Vector3, int> syncPositionEvent;
	protected override void process(EzyApp app, EzyArray data)
	{
		logger.info("Sync position: " + data);
		string playerName = data.get<string>(0);
		EzyArray positionArray = data.get<EzyArray>(1);
		EzyArray rotationArray = data.get<EzyArray>(2);
		int time = data.get<int>(3);
		Vector3 position = new Vector3(positionArray.get<float>(0),
		                               positionArray.get<float>(1),
		                               positionArray.get<float>(2));

		Vector3 rotation = new Vector3(rotationArray.get<float>(0),
		                               rotationArray.get<float>(1),
		                               rotationArray.get<float>(2));

		syncPositionEvent?.Invoke(playerName, position, rotation, time);
	}
}

class PlayerBeingAttackedHandler : EzyAbstractAppDataHandler<EzyObject>
{
	public static event Action<string, string> playersBeingAttackedEvent;
	protected override void process(EzyApp app, EzyObject data)
	{
		logger.info("Being Attacked: " + data);
		var playerBeingAttacked = data.get<string>("b");
		var attackTime = data.get<float>("t");
		var attackerName = data.get<string>("a");
		var attackPosition = data.get<EzyArray>("p");
		logger.info("playerBeingAttacked: " + playerBeingAttacked);
		logger.info("Attacker: " + attackerName);
		playersBeingAttackedEvent?.Invoke(playerBeingAttacked, attackerName);
	}
}

class PlayerAttackDataHandler : EzyAbstractAppDataHandler<EzyObject>
{
	public static event Action<string> playerAttackEvent;
	protected override void process(EzyApp app, EzyObject data)
	{
		logger.info("Attack: " + data);
		var attackerName = data.get<string>("a");
		playerAttackEvent?.Invoke(attackerName);
	}

}

#endregion

public class SocketProxy : EzyLoggable
{
	private static readonly SocketProxy INSTANCE = new SocketProxy();

	public const string ZONE_NAME = "EzySmashers";
	public const string APP_NAME = "EzySmashers";

	private EzyUTClient client;
	private User userAuthenInfo = new User("test", "test1234");
	private string host;
	private int port;

	public EzyUTClient Client { get => client; }
	public User UserAuthenInfo { get => userAuthenInfo; set => userAuthenInfo = value; }
	public string Host { get => host; }
	public int Port { get => port; }

	public static SocketProxy getInstance()
	{
		return INSTANCE;
	}

	public EzyUTClient setup(string host, int port)
	{
		this.host = host;
		this.port = port;

		logger.debug("Set up socket client");
		var config = EzyClientConfig.builder()
			.clientName(ZONE_NAME)
			.build();

		var clients = EzyClients.getInstance();
		client = new EzyUTClient(config);
		clients.addClient(client);

		var setup = client.setup();

		setup.addDataHandler(EzyCommand.HANDSHAKE, new HandshakeHandler());
		setup.addDataHandler(EzyCommand.LOGIN, new LoginSuccessHandler());
		setup.addDataHandler(EzyCommand.UDP_HANDSHAKE, new UdpHandshakeHandler());
		setup.addDataHandler(EzyCommand.APP_ACCESS, new AppAccessHandler());

		// Set up EzySmashers app
		var appSetup = setup.setupApp(APP_NAME);
		appSetup.addDataHandler(Commands.JOIN_LOBBY, new JoinLobbyResponseHandler());
		appSetup.addDataHandler(Commands.CREATE_MMO_ROOM, new CreateRoomResponseHandler());
		appSetup.addDataHandler(Commands.GET_MMO_ROOM_ID_LIST, new GetMMORoomIdListResponse());
		appSetup.addDataHandler(Commands.GET_MMO_ROOM_PLAYERS, new GetMMORoomPlayersResponseHandler());
		appSetup.addDataHandler(Commands.JOIN_MMO_ROOM, new JoinMMORoomResponseHandler());
		appSetup.addDataHandler(Commands.ANOTHER_JOIN_MMO_ROOM, new AnotherJoinMMORoomHandler());
		appSetup.addDataHandler(Commands.ANOTHER_EXIT_MMO_ROOM, new AnotherExitMMORoomHandler());
		appSetup.addDataHandler(Commands.START_GAME, new StartGameResponseHandler());
		appSetup.addDataHandler(Commands.SYNC_POSITION, new SyncPositionHandler());
		appSetup.addDataHandler(Commands.PLAYER_BEING_ATTACKED, new PlayerBeingAttackedHandler());
		appSetup.addDataHandler(Commands.PLAYER_ATTACK_DATA, new PlayerAttackDataHandler());

		// Init RoomManager
		RoomManager.getInstance();

		return client;
	}

	public void login(string username, string password)
	{
		userAuthenInfo.Username = username;
		userAuthenInfo.Password = password;
		client.connect(host, port);
	}
}
