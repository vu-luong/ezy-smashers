using System;
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
        logger.debug("UdpHandshakeHandler den day roi");
        SocketRequest.getInstance().sendAppAccessRequest();
    }
}

class AppAccessHandler : EzyAppAccessHandler
{
    public static event Action socketSetupCompletedEvent;
    protected override void postHandle(EzyApp app, EzyArray data)
    {
        logger.debug("Completed setting up socket client");
        socketSetupCompletedEvent?.Invoke();
    }
}

#region App Data Handler



#endregion

public class SocketProxy : EzyLoggable
{
    private static readonly SocketProxy INSTANCE = new SocketProxy();

    public const string ZONE_NAME = "EzyTank";
    public const string APP_NAME = "EzyTank";

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

        // Set up ezytank app
        var appSetup = setup.setupApp(APP_NAME);

        return client;
    }

    public void login(string username, string password)
    {
        userAuthenInfo.Username = username;
        userAuthenInfo.Password = password;
        client.connect(host, port);
    }

}
