using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LobbyController : MonoBehaviour
{
    public UnityEvent<List<long>> mmoRoomIdListUpdateEvent;

    private void Awake()
    {
        CreateRoomResponseHandler.roomCreatedEvent += OnRoomCreated;
        GetMMORoomIdListResponse.mmoRoomIdListResponseEvent += OnMMORoomIdListResponse;
    }

    private void Start()
    {
        OnRefreshRoomIdList();
    }

    public void OnCreateMMORoom()
    {
        Debug.Log("LobbyController: OnCreateMMORoom!");
        SocketRequest.getInstance().sendCreateMMORoomRequest();
    }

    public void OnRoomCreated(long roomId, bool master)
    {
        // Change scene here
        SceneManager.LoadScene("GameLoungeScene");
    }

    public void OnRefreshRoomIdList() 
    {
        Debug.Log("LobbyController: OnRefreshRoomIdList");
        SocketRequest.getInstance().sendGetMMORoomIdListRequest();
    }

    public void OnMMORoomIdListResponse(List<long> roomIdList) {
        mmoRoomIdListUpdateEvent?.Invoke(roomIdList);
    }
}
