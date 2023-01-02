using System;
using com.tvd12.ezyfoxserver.client.support;
using UnityEngine.SceneManagement;

public class LoginController : DefaultMonoBehaviour
{
	public StringVariable username;
	public StringVariable password;

	private void Start()
	{
		AddHandler<Object>(Commands.JOIN_LOBBY, OnJoinedLobby);
	}

	public void Login()
	{
		// Login to socket server
		DefaultSocketManager.GetInstance()
			.Login(username.Value, password.Value);
	}

	void OnJoinedLobby(EzyAppProxy appProxy, Object data)
	{
		GameManager.getInstance().SetUpMyPlayer(username.Value);
		SceneManager.LoadScene("LobbyScene");
	}
}
