using com.tvd12.ezyfoxserver.client.constant;
using com.tvd12.ezyfoxserver.client.logger;
using com.tvd12.ezyfoxserver.client.support;
using UnityEngine;

public class DefaultSocketManager
{
    private static DefaultSocketManager INSTANCE = new();
    private readonly SocketConfigVariable socketConfig
        = (SocketConfigVariable)Resources.Load("SocketConfig");
    private EzyLogger logger;
    private EzySocketProxy socketProxy;
    private EzyAppProxy appProxy;

    public EzyAppProxy AppProxy => appProxy;

    public static DefaultSocketManager GetInstance()
    {
        return INSTANCE;
    }
    
    private DefaultSocketManager()
    {
        Init();
    }

    void Init()
    {
        // Enable EzyLogger
        EzyLoggerFactory.setLoggerSupply(type => new UnityLogger(type));
        logger = EzyLoggerFactory.getLogger<DefaultSocketManager>();

        // Set up socket client
        SetupSocketProxy();
    }
    
    private void SetupSocketProxy()
    {
        logger.debug("Setting up socket proxy");
        SocketProxyManager socketProxyManager = SocketProxyManager.getInstance();
        socketProxyManager.setDefaultZoneName(socketConfig.Value.ZoneName);
        socketProxyManager.init();

        socketProxy = socketProxyManager.getDefaultSocketProxy()
            .setTransportType(EzyTransportType.UDP)
            .setDefaultAppName(socketConfig.Value.AppName);

        socketProxy.onLoginSuccess<object>(HandleLoginSuccess);
        socketProxy.onAppAccessed<object>(HandleAppAccessed);

        appProxy = socketProxy.getDefaultAppProxy();
    }
    
    private void HandleLoginSuccess(EzySocketProxy proxy, object data)
    {
        logger.debug("Log in successfully");
        socketProxy.getClient().udpConnect(socketConfig.Value.UdpPort);
    }
    
    private void HandleAppAccessed(EzyAppProxy proxy, object data)
    {
        logger.debug("App access successfully");
        SocketRequest.getInstance().SendJoinLobbyRequest();
    }

    public void Login(string username, string password)
    {
        SocketProxyManager.getInstance()
            .getDefaultSocketProxy()
            .setHost(socketConfig.Value.Host)
            .setLoginUsername(username)
            .setLoginPassword(password)
            .connect();
    }
}
