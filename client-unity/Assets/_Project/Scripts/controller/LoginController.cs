using com.tvd12.ezyfoxserver.client.support;
using com.tvd12.ezyfoxserver.client.unity;
using UnityEngine;
using UnityEngine.Events;
using Object = System.Object;

public class LoginController : EzyDefaultController
{
	[SerializeField]
	private StringVariable username;
	
	[SerializeField]
	private StringVariable password;
	
	[SerializeField]
	private string host;
	
	[SerializeField]
	private int udpPort;

	[SerializeField]
	private UnityEvent<string> myPlayerJoinedLobbyEvent;

	private void Awake()
	{
		base.Awake();
		AddHandler<Object>(Commands.JOIN_LOBBY, OnJoinedLobby);
	}

	public void Login()
	{
		// Login to socket server
		EzySingletonSocketManager.GetInstance()
			.SetLoginSuccessHandler(HandleLoginSuccess);
		EzySingletonSocketManager.GetInstance()
			.SetAppAccessedHandler(HandleAppAccessed);
		EzySingletonSocketManager.GetInstance()
			.Login(host, username.Value, password.Value);
	}
	
	private void HandleLoginSuccess(EzySocketProxy proxy, object data)
	{
		logger.debug("Log in successfully");
		proxy.getClient().udpConnect(udpPort);
	}
    
	private void HandleAppAccessed(EzyAppProxy proxy, object data)
	{
		logger.debug("App access successfully");
		SocketRequest.GetInstance().SendJoinLobbyRequest();
	}

	void OnJoinedLobby(EzyAppProxy appProxy, Object data)
	{
		myPlayerJoinedLobbyEvent?.Invoke(username.Value);
	}
}
