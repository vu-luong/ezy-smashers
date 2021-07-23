using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginController : MonoBehaviour
{
    public StringVariable username;
    public StringVariable password;

    private void Awake()
    {
        PluginInfoHandler.socketSetupCompletedEvent += OnSocketSetupCompleted;
    }

    public void OnLogin()
    {
        // Login to socket server
        SocketProxy.getInstance().login(username.Value, password.Value);
    }

    void OnSocketSetupCompleted()
    {
        // Change scene here
        SceneManager.LoadScene("MainScene");
    }
}
