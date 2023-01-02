using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client.entity;
using com.tvd12.ezyfoxserver.client.support;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameLoungeController : DefaultMonoBehaviour
{
    public UnityEvent<string> setRoomTitleEvent;
    public UnityEvent updateRoomPlayersEvent;

    private void Start()
    {
        AddHandler<EzyObject>(Commands.GET_MMO_ROOM_PLAYERS, OnGetMMORoomPlayersResponse);
        AddHandler<EzyObject>(Commands.ANOTHER_JOIN_MMO_ROOM, OnAnotherJoinMMORoom);
        AddHandler<EzyObject>(Commands.ANOTHER_EXIT_MMO_ROOM, OnAnotherExitMMORoom);
        AddHandler<EzyArray>(Commands.START_GAME, OnGameStarted);
        SetRoomTitle();
        GetMMORoomPlayers();
    }

    private void GetMMORoomPlayers()
    {
        SocketRequest.getInstance()
            .SendGetMMORoomPlayersRequest();
    }

    private void SetRoomTitle()
    {
        long currentRoomId = RoomManager.GetInstance().CurrentRoomId;
        setRoomTitleEvent?.Invoke("Room #" + currentRoomId);
    }

    private void OnGetMMORoomPlayersResponse(EzyAppProxy proxy, EzyObject data) 
    {
        List<string> playerNames = data.get<EzyArray>("players").toList<string>();
        string masterName = data.get<string>("master");
        logger.debug("OnGetMMORoomPlayersResponse");
        logger.debug("Player Names: " + string.Join(",", playerNames));
        logger.debug("Master Name: " + masterName);
        RoomManager.GetInstance().SetCurrentRoomPlayers(playerNames, masterName);
        updateRoomPlayersEvent.Invoke();
    }

    private void OnAnotherJoinMMORoom(EzyAppProxy proxy, EzyObject data) 
    {
        string anotherPlayerName = data.get<string>("playerName");
        logger.debug("OnAnotherJoinMMORoom anotherPlayerName = " + anotherPlayerName);
        GetMMORoomPlayers();
    }

    private void OnAnotherExitMMORoom(EzyAppProxy proxy, EzyObject data) 
    {
        string anotherName = data.get<string>("playerName");
        logger.debug("OnAnotherExitMMORoom anotherPlayerName = " + anotherName);
        GetMMORoomPlayers();
    }

    private void OnGameStarted(EzyAppProxy proxy, EzyArray data)
    { 
        logger.debug("OnGameStart");
        List<PlayerSpawnInfoModel> spawnData = new List<PlayerSpawnInfoModel>();

        for (int i = 0; i < data.size(); i++)
        {
            EzyObject item = data.get<EzyObject>(i);
            string playerName = item.get<string>("playerName");
            List<float> position = item.get<EzyArray>("position").toList<float>();
            List<float> color = item.get<EzyArray>("color").toList<float>();
            spawnData.Add(
                new PlayerSpawnInfoModel(
                    playerName,
                    new Vector3(position[0], position[1], position[2]),
                    new Vector3(color[0], color[1], color[2])
                )
            );
        }
        GameManager.getInstance().PlayersSpawnInfo = spawnData;
        SceneManager.LoadScene("MainScene");
    }

    #region public methods

    public void StartGame() {
        SocketRequest.getInstance()
            .SendStartGameRequest();
    }

    #endregion
}
