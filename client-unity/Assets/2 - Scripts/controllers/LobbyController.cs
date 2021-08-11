using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LobbyController : MonoBehaviour
{
    public UnityEvent<List<int>> mmoRoomIdListUpdateEvent;

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

    public void OnRoomCreated(long roomId)
    {
        RoomManager.getInstance().CurrentRoomId = roomId;
        SceneManager.LoadScene("GameLoungeScene");
    }

    public void OnRefreshRoomIdList() 
    {
        Debug.Log("LobbyController: OnRefreshRoomIdList");
        SocketRequest.getInstance().sendGetMMORoomIdListRequest();
    }

    public void OnMMORoomIdListResponse(List<int> roomIdList) {
        mmoRoomIdListUpdateEvent?.Invoke(roomIdList);
    }
}
