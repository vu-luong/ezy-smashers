using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LobbyController : MonoBehaviour
{
    public UnityEvent<List<String>> mmoRoomListUpdateEvent;

    private void Awake()
    {
        CreateRoomResponseHandler.roomCreatedEvent += OnRoomCreated;
        GetMMORoomListResponse.mmoRoomListResponseEvent += OnMMORoomListResponse;
    }

    private void Start()
    {
        OnRefreshRoomList();
    }

    public void OnCreateMMORoom()
    {
        Debug.Log("LobbyController: OnCreateMMORoom!");
        SocketRequest.getInstance().sendCreateMMORoomRequest();
    }

    public void OnRoomCreated()
    {
        // Change scene here
        SceneManager.LoadScene("GameLoungeScene");
    }

    public void OnRefreshRoomList() 
    {
        Debug.Log("LobbyController: OnRefreshRoomList");
        SocketRequest.getInstance().sendGetMMORoomListRequest();
    }

    public void OnMMORoomListResponse(List<String> roomList) {
        mmoRoomListUpdateEvent?.Invoke(roomList);
    }
}
