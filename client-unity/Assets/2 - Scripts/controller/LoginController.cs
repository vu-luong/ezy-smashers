using com.tvd12.ezyfoxserver.client.support;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = System.Object;

public class LoginController : EzyDefaultController
{
	public StringVariable username;
	public StringVariable password;
	[SerializeField] private string host;
	[SerializeField] private int udpPort;

	private void Awake()
	{
		AddHandler<Object>(Commands.JOIN_LOBBY, OnJoinedLobby);
	}

	public void Login()
	{
		// Login to socket server
		EzyDefaultSocketManager.GetInstance()
			.Login(host, username.Value, password.Value, HandleLoginSuccess, HandleAppAccessed);
	}
	
	private void HandleLoginSuccess(EzySocketProxy proxy, object data)
	{
		logger.debug("Log in successfully");
		proxy.getClient().udpConnect(udpPort);
	}
    
	private void HandleAppAccessed(EzyAppProxy proxy, object data)
	{
		logger.debug("App access successfully");
		SocketRequest.getInstance().SendJoinLobbyRequest();
	}

	void OnJoinedLobby(EzyAppProxy appProxy, Object data)
	{
		GameManager.GetInstance().SetUpMyPlayer(username.Value);
		SceneManager.LoadScene("LobbyScene");
	}
}
