using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LobbyController : MonoBehaviour
{
    public UnityEvent<List<int>> mmoRoomIdListUpdateEvent;

    private void Awake()
    {
        CreateRoomResponseHandler.roomCreatedEvent += JoinRoom;
        GetMMORoomIdListResponse.mmoRoomIdListResponseEvent += OnMMORoomIdListResponse;
        JoinMMORoomResponse.joinRoomResponseEvent += JoinRoom;
    }

    private void Start()
    {
        OnRefreshRoomIdList();
    }

    private void JoinRoom(int roomId)
    {
        RoomManager.getInstance().CurrentRoomId = roomId;
        SceneManager.LoadScene("GameLoungeScene");
    }

    private void OnMMORoomIdListResponse(List<int> roomIdList) {
        mmoRoomIdListUpdateEvent?.Invoke(roomIdList);
    }

    #region public methods
    public void OnRefreshRoomIdList() 
    {
        Debug.Log("LobbyController: OnRefreshRoomIdList");
        SocketRequest.getInstance().sendGetMMORoomIdListRequest();
    }

    public void OnCreateMMORoom()
    {
        Debug.Log("LobbyController: OnCreateMMORoom!");
        SocketRequest.getInstance().sendCreateMMORoomRequest();
    }

    public void RequestJoinMMORoom(int roomId) {
        SocketRequest.getInstance().sendJoinMMORoomRequest(roomId);
    }
    #endregion
}
