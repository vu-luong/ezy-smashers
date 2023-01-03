using com.tvd12.ezyfoxserver.client.constant;
using com.tvd12.ezyfoxserver.client.logger;
using com.tvd12.ezyfoxserver.client.support;
using UnityEngine;

public class EzyDefaultSocketManager
{
    private static EzyDefaultSocketManager INSTANCE = new();
    private readonly SocketConfigVariable socketConfig
        = (SocketConfigVariable)Resources.Load("SocketConfig");
    private EzyLogger logger;
    private EzySocketProxy socketProxy;
    private EzyAppProxy appProxy;

    public EzySocketProxy SocketProxy => socketProxy;
    public EzyAppProxy AppProxy => appProxy;

    public static EzyDefaultSocketManager GetInstance()
    {
        return INSTANCE;
    }
    
    private EzyDefaultSocketManager()
    {
        Init();
    }

    void Init()
    {
        // Enable EzyLogger
        EzyLoggerFactory.setLoggerLevel(EzyLoggerLevel.INFO);
        EzyLoggerFactory.setLoggerSupply(type => new UnityLogger(type));
        logger = EzyLoggerFactory.getLogger<EzyDefaultSocketManager>();

        // Set up socket proxy and app proxy
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

        appProxy = socketProxy.getDefaultAppProxy();
    }
    
    public void Login(
        string host,
        string username,
        string password,
        EzySocketProxyDataHandler<object> loginSuccessHandler,
        EzyAppProxyDataHandler<object> appAccessedHandler)
    {
        socketProxy.onLoginSuccess(loginSuccessHandler);
        socketProxy.onAppAccessed(appAccessedHandler);
        
        SocketProxyManager.getInstance()
            .getDefaultSocketProxy()
            .setHost(host)
            .setLoginUsername(username)
            .setLoginPassword(password)
            .connect();
    }
}
