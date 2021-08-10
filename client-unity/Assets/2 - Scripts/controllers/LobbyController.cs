using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyController : MonoBehaviour
{

    private void Awake()
    {
        CreateRoomResponseHandler.roomCreatedEvent += OnRoomCreated;
    }

    public void OnCreateMMORoom()
    {
        Debug.Log("LobbyController: OnCreateMMORoom!");
        SocketRequest.getInstance().sendCreateMMORoomRequest();
    }

    public void OnRoomCreated()
    {
        // Change scene here
        //SceneManager.LoadScene("GameLoungeScene");
    }

    public void OnRefreshRoomList() 
    {
        Debug.Log("LobbyController: OnRefreshRoomList");
        SocketRequest.getInstance().sendGetMMORoomListRequest();
    }
}
