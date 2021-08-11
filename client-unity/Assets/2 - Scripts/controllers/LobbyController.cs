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

    public void JoinRoom(int roomId)
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

    public void RequestJoinRoom(int roomId) {
        // TODO: send request to join a room
    }

    public void RequestJoinRoomResponse(int roomId) { 
        // TODO: listen to join_room response
        // JoinRoom(roomId);
    }
}
