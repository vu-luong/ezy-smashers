using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginController : MonoBehaviour
{
    public StringVariable username;
    public StringVariable password;

    private void Awake()
    {
        JoinLobbyResponseHandler.joinedLobbyEvent += OnJoinedLobby;
    }

    public void OnLogin()
    {
        // Login to socket server
        SocketProxy.getInstance().login(username.Value, password.Value);
    }

    void OnJoinedLobby()
    {
        GameManager.getInstance().SetUpPlayer();

        // Change scene here
        SceneManager.LoadScene("LobbyScene");
    }
}
