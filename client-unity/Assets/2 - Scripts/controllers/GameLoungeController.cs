using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameLoungeController : MonoBehaviour
{
    public UnityEvent<string> setRoomTitleEvent;
    public UnityEvent updateRoomPlayersEvent;

    private void Awake()
    {
        GetMMORoomPlayersResponse.mmoRoomPlayersResponseEvent += OnGetMMORoomPlayersResponse;
        SetRoomTitle();
        GetMMORoomPlayers();
    }

    private void GetMMORoomPlayers()
    {
        SocketRequest.getInstance().sendGetMMORoomPlayersRequest();
    }

    private void SetRoomTitle()
    {
        long currentRoomId = RoomManager.getInstance().CurrentRoomId;
        setRoomTitleEvent?.Invoke("Room #" + currentRoomId);
    }

    private void OnGetMMORoomPlayersResponse(List<string> playerNames, string master) 
    {
        Debug.Log("GameLoungeController.OnGetMMORoomPlayersResponse");
        RoomManager.getInstance().SetCurrentRoomPlayers(playerNames, master);

        updateRoomPlayersEvent.Invoke();
    }
}
