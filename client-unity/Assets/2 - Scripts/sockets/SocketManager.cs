using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client.constant;
using com.tvd12.ezyfoxserver.client.logger;
using com.tvd12.ezyfoxserver.client.support;
using UnityEngine;

public class SocketManager : MonoBehaviour
{
    private const string ZONE_NAME = "EzySmashers";
    private const string APP_NAME = "EzySmashers";
    private static SocketManager _instance;
    public string host = "127.0.0.1";
    private EzyLogger logger;
    private EzySocketProxy socketProxy;
    private EzyAppProxy appProxy;
    private readonly List<object> handlers = new();

    public EzySocketProxy SocketProxy => socketProxy;
    public EzyAppProxy AppProxy => appProxy;

    public SocketManager()
    {
        if (_instance == null)
        {
            Init();
        }
    }

    private void Awake()
    {
        // If go back to current scene, don't make duplication
        if (_instance != null)
        {
            Destroy(gameObject);
        } else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public static SocketManager GetInstance()
    {
        return _instance;
    }

    void Init()
    {
        // Enable EzyLogger
        EzyLoggerFactory.setLoggerSupply(type => new UnityLogger(type));
        logger = EzyLoggerFactory.getLogger<SocketManager>();

        // Set up socket client
        SetupSocketProxy();
    }
    
    private void SetupSocketProxy()
    {
        SocketProxyManager socketProxyManager = SocketProxyManager.getInstance();
        socketProxyManager.setDefaultZoneName(ZONE_NAME);
        socketProxyManager.init();

        socketProxy = socketProxyManager.getDefaultSocketProxy()
            .setTransportType(EzyTransportType.UDP)
            .setHost(host)
            .setDefaultAppName(APP_NAME);

        handlers.Add(socketProxy.onLoginSuccess<object>(HandleLoginSuccess));
        handlers.Add(socketProxy.onAppAccessed<object>(HandleAppAccessed));

        appProxy = socketProxy.getDefaultAppProxy();
    }
    
    private void HandleLoginSuccess(EzySocketProxy proxy, object data)
    {
        logger.debug("Log in successfully");
        socketProxy.getClient().udpConnect(2611);
    }
    
    private void HandleAppAccessed(EzyAppProxy proxy, object data)
    {
        logger.debug("App access successfully");
        SocketRequest.getInstance().SendJoinLobbyRequest();
    }

    // Update is called once per frame
    void Update()
    {
        // Main thread pulls data from socket
        SocketProxyManager.getInstance()
            .getDefaultSocketProxy()
            .getClient()
            .processEvents();
    }

    public void login(string username, string password)
    {
        SocketProxyManager.getInstance()
            .getDefaultSocketProxy()
            .setLoginUsername(username)
            .setLoginPassword(password)
            .connect();
    }

    private void OnDestroy()
    {
        foreach (object handler in handlers)
        {
            socketProxy.unbind(handler);
        }
    }
}
