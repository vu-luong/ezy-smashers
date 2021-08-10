using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LobbyController : MonoBehaviour
{
    public UnityEvent<List<String>> mmoRoomNamesUpdateEvent;

    private void Awake()
    {
        CreateRoomResponseHandler.roomCreatedEvent += OnRoomCreated;
        GetMMORoomNamesResponse.mmoRoomNamesResponseEvent += OnMMORoomNamesResponse;
    }

    private void Start()
    {
        OnRefreshRoomNames();
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

    public void OnRefreshRoomNames() 
    {
        Debug.Log("LobbyController: OnRefreshRoomNames");
        SocketRequest.getInstance().sendGetMMORoomNamesRequest();
    }

    public void OnMMORoomNamesResponse(List<String> roomNames) {
        mmoRoomNamesUpdateEvent?.Invoke(roomNames);
    }
}
