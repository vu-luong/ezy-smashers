using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginController : MonoBehaviour
{
	public StringVariable username;
	public StringVariable password;
	// public float fixedDeltaTime = 0.02f;

	private void Awake()
	{
		// Time.fixedDeltaTime = fixedDeltaTime;
		JoinLobbyResponseHandler.joinedLobbyEvent += OnJoinedLobby;
	}

	private void UnregisterEvents()
	{
		JoinLobbyResponseHandler.joinedLobbyEvent -= OnJoinedLobby;
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
