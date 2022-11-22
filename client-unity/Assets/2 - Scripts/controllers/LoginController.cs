using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginController : MonoBehaviour
{
	public StringVariable username;
	public StringVariable password;

	private void Awake()
	{
		JoinLobbyResponseHandler.action += OnJoinedLobby;
	}

	private void UnregisterEvents()
	{
		JoinLobbyResponseHandler.action -= OnJoinedLobby;
	}
	
	public void OnLogin()
	{
		// Login to socket server
		SocketProxy.getInstance().login(username.Value, password.Value);
	}

	void OnJoinedLobby()
	{
		GameManager.getInstance().SetUpPlayer();
		UnregisterEvents();
		// Change scene here
		SceneManager.LoadScene("LobbyScene");
	}
}
