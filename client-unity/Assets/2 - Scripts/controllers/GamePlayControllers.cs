using System.Collections.Generic;
using _2___Scripts.shared;
using Cinemachine;
using UnityEngine;

public class GamePlayControllers : MonoBehaviour
{
	
	// public ClientPlayer mePlayer;
	public GameObject playerPrefab;
	private Dictionary<string, ClientPlayer> playersMap = new Dictionary<string, ClientPlayer>();
	public CinemachineVirtualCamera cinemachineVirtualCamera;

	private void Awake()
	{
		SpawnPlayers(GameManager.getInstance().PlayersSpawnData);
		ClientPlayer.playerInputEvent += OnPlayerInputChange;
		SyncPositionHandler.syncPositionEvent += OnPlayerSyncPosition;
	}
	private void SpawnPlayers(List<PlayerSpawnData> playersSpawnData)
	{
		foreach (var playerSpawnData in playersSpawnData)
		{
			SpawnPlayer(playerSpawnData);
		}
	}
	private void SpawnPlayer(PlayerSpawnData playerSpawnData)
	{
		GameObject go = Instantiate(playerPrefab);
		ClientPlayer clientPlayer = go.GetComponent<ClientPlayer>();
		clientPlayer.Initialize(playerSpawnData);
		playersMap.Add(playerSpawnData.playerName, clientPlayer);
		if (playerSpawnData.playerName == GameManager.getInstance().MyPlayer.PlayerName)
		{
			cinemachineVirtualCamera.Follow = go.transform;
		}
	}

	private void OnPlayerSyncPosition(string playerName, Vector3 position, Vector3 rotation, int time)
	{
		if (playerName == GameManager.getInstance().MyPlayer.PlayerName)
		{
			playersMap[playerName].OnServerDataUpdate(position, time);
		}
		else
		{
			// TODO
		}
	}
	
	private void OnPlayerInputChange(PlayerInputData inputData)
	{
		SocketRequest.getInstance().SendPlayerInputData(inputData);
	}
}
