using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameLoungeController : MonoBehaviour
{
    public UnityEvent<string> setRoomTitleEvent;
    public UnityEvent updateRoomPlayersEvent;

    private void Awake()
    {
        GetMMORoomPlayersResponseHandler.mmoRoomPlayersResponseEvent += OnGetMMORoomPlayersResponse;
        AnotherJoinMMORoomHandler.anotherJoinMMORoomEvent += OnAnotherJoinMMORoom;
        AnotherExitMMORoomHandler.anotherExitMMORoomEvent += OnAnotherExitMMORoom;
        StartGameResponseHandler.startGameResponseEvent += OnGameStart;
        SetRoomTitle();
        GetMMORoomPlayers();
    }

    private void UnregisterEvents()
    {
        GetMMORoomPlayersResponseHandler.mmoRoomPlayersResponseEvent -= OnGetMMORoomPlayersResponse;
        AnotherJoinMMORoomHandler.anotherJoinMMORoomEvent -= OnAnotherJoinMMORoom;
        AnotherExitMMORoomHandler.anotherExitMMORoomEvent -= OnAnotherExitMMORoom;
        StartGameResponseHandler.startGameResponseEvent -= OnGameStart;
    }

    private void GetMMORoomPlayers()
    {
        SocketRequest.getInstance().SendGetMMORoomPlayersRequest();
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

    private void OnAnotherJoinMMORoom(string anotherName) 
    {
        Debug.Log("GameLoungeController.OnAnotherJoinMMORoom");
        GetMMORoomPlayers();
    }

    private void OnAnotherExitMMORoom(string anotherName) 
    {
        Debug.Log("GameLoungeController.OnAnotherExitMMORoom");
        GetMMORoomPlayers();
    }

    private void OnGameStart(List<PlayerSpawnData> playersSpawnData)
    { 
        Debug.Log("GameLoungeController.OnGameStart");

        GameManager.getInstance().PlayersSpawnData = playersSpawnData;
        UnregisterEvents();
        SceneManager.LoadScene("MainScene");
    }

    #region public methods

    public void SendStartGameRequest() {
        SocketRequest.getInstance().SendStartGameRequest();
    }

    #endregion
}
