using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyController : MonoBehaviour
{

    private void Awake()
    {
        
    }

    public void OnCreateMMORoom()
    {
        Debug.Log("LobbyController: OnCreateMMORoom!");
        SocketRequest.getInstance().sendCreateMMORoomRequest();
    }
}
