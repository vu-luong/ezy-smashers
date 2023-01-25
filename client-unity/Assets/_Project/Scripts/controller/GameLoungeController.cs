using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client.entity;
using com.tvd12.ezyfoxserver.client.support;
using com.tvd12.ezyfoxserver.client.unity;
using UnityEngine;
using UnityEngine.Events;

public class GameLoungeController : EzyDefaultController
{
    [SerializeField]
    private UnityEvent<List<PlayerModel>> updateRoomPlayersEvent;
    
    [SerializeField]
    private UnityEvent<List<PlayerSpawnInfoModel>> gameStartedEvent;
    
    

    private void Start()
    {
        base.Start();
        appProxy.on<EzyObject>(Commands.GET_MMO_ROOM_PLAYERS, OnGetMMORoomPlayersResponse);
        appProxy.on<EzyObject>(Commands.ANOTHER_JOIN_MMO_ROOM, OnAnotherJoinMMORoom);
        appProxy.on<EzyObject>(Commands.ANOTHER_EXIT_MMO_ROOM, OnAnotherExitMMORoom);
        appProxy.on<EzyArray>(Commands.START_GAME, OnGameStarted);
        GetMMORoomPlayers();
    }

    private void GetMMORoomPlayers()
    {
        appProxy.send(Commands.GET_MMO_ROOM_PLAYERS);
    }

    private void OnGetMMORoomPlayersResponse(EzyAppProxy proxy, EzyObject data) 
    {
        List<string> playerNames = data.get<EzyArray>("players").toList<string>();
        string masterName = data.get<string>("master");
        LOGGER.debug("OnGetMMORoomPlayersResponse");
        LOGGER.debug("Player Names: " + string.Join(",", playerNames));
        LOGGER.debug("Master Name: " + masterName);
        List<PlayerModel> players = new();
        foreach (string playerName in playerNames)
        {
            bool isMaster = playerName.Equals(masterName);
            players.Add(new PlayerModel(playerName, isMaster));
        }
        updateRoomPlayersEvent?.Invoke(players);
    }

    private void OnAnotherJoinMMORoom(EzyAppProxy proxy, EzyObject data) 
    {
        string anotherPlayerName = data.get<string>("playerName");
        LOGGER.debug("OnAnotherJoinMMORoom anotherPlayerName = " + anotherPlayerName);
        GetMMORoomPlayers();
    }

    private void OnAnotherExitMMORoom(EzyAppProxy proxy, EzyObject data) 
    {
        string anotherName = data.get<string>("playerName");
        LOGGER.debug("OnAnotherExitMMORoom anotherPlayerName = " + anotherName);
        GetMMORoomPlayers();
    }

    private void OnGameStarted(EzyAppProxy proxy, EzyArray data)
    { 
        LOGGER.debug("OnGameStart");
        List<PlayerSpawnInfoModel> spawnInfos = new List<PlayerSpawnInfoModel>();

        for (int i = 0; i < data.size(); i++)
        {
            EzyObject item = data.get<EzyObject>(i);
            string playerName = item.get<string>("playerName");
            EzyArray position = item.get<EzyArray>("position");
            EzyArray color = item.get<EzyArray>("color");
            spawnInfos.Add(
                new PlayerSpawnInfoModel(
                    playerName,
                    new Vector3(position.get<float>(0), position.get<float>(1), position.get<float>(2)),
                    new Vector3(color.get<float>(0), color.get<float>(1), color.get<float>(2))
                )
            );
        }
        gameStartedEvent?.Invoke(spawnInfos);
    }

    #region public methods

    public void StartGame() {
        appProxy.send(Commands.START_GAME);
    }

    #endregion
}
