using System;
using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client.util;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : EzyLoggable
{
    private static readonly RoomManager INSTANCE = new RoomManager();
    private List<Player> currentRoomPlayers;
    private long currentRoomId;

    public long CurrentRoomId { get => currentRoomId; set => currentRoomId = value; }
    public List<Player> CurrentRoomPlayers { get => currentRoomPlayers; }

    public RoomManager()
    {
    }

    public static RoomManager getInstance()
    {
        return INSTANCE;
    }

    public void SetCurrentRoomPlayers(List<string> playerNames, string master)
    {
        Debug.Log("RoomManager.OnGetMMORoomPlayersResponse");
        currentRoomPlayers = new List<Player>();
        foreach (string playerName in playerNames)
        {
            Player player = new Player(playerName);
            player.IsMaster = (playerName.Equals(master));
            currentRoomPlayers.Add(player);
        }
    }

    public void SetCurrentRoomId(long roomId)
    {
        Debug.Log("RoomManager.setCurrentRoomId");
        currentRoomId = roomId;
    }

    public void ExitCurrentRoom()
    { 
        // TODO
    }
}
