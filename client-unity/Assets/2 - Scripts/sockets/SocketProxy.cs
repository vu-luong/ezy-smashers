using System;
using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client;
using com.tvd12.ezyfoxserver.client.config;
using com.tvd12.ezyfoxserver.client.constant;
using com.tvd12.ezyfoxserver.client.entity;
using com.tvd12.ezyfoxserver.client.handler;
using com.tvd12.ezyfoxserver.client.request;
using com.tvd12.ezyfoxserver.client.util;

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
        SocketRequest.getInstance().sendAppAccessRequest();
    }
}

class AppAccessHandler : EzyAppAccessHandler
{
    protected override void postHandle(EzyApp app, EzyArray data)
    {
        logger.debug("App access successfully");
        SocketRequest.getInstance().sendJoinLobbyRequest();
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
    public static event Action roomCreatedEvent;
    protected override void process(EzyApp app, EzyObject data)
    {
        logger.info("Room created successfully: " + data.ToString());
        roomCreatedEvent?.Invoke();
    }
}

class GetMMORoomListResponse : EzyAbstractAppDataHandler<EzyArray>
{
    public static event Action<List<string>> mmoRoomListResponseEvent;
    protected override void process(EzyApp app, EzyArray data)
    {
        logger.info("Room list: " + data.get<EzyArray>(0).ToString());
        List<string> roomList = data.get<EzyArray>(0).toList<string>();
        mmoRoomListResponseEvent?.Invoke(roomList);
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
        appSetup.addDataHandler(Commands.GET_MMO_ROOM_LIST, new GetMMORoomListResponse());

        return client;
    }

    public void login(string username, string password)
    {
        userAuthenInfo.Username = username;
        userAuthenInfo.Password = password;
        client.connect(host, port);
    }

}
