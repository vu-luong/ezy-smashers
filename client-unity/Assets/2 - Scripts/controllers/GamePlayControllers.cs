using System.Collections.Generic;
using _2___Scripts.shared;
using Cinemachine;
using UnityEngine;

public class GamePlayControllers : MonoBehaviour
{
	public GameObject playerPrefab;
	private Dictionary<string, ClientPlayer> playersMap = new Dictionary<string, ClientPlayer>();
	public CinemachineVirtualCamera cinemachineVirtualCamera;

	public Dictionary<string, ClientPlayer> PlayersMap => playersMap;

	private void Awake()
	{
		SpawnPlayers(GameManager.getInstance().PlayersSpawnData);
		ClientPlayer.playerInputEvent += OnPlayerInputChange;
		// ClientPlayer.playerAttackEvent += OnPlayerAttack;
		Hammer.playerHitEvent += OnPlayerAttack;
		SyncPositionHandler.syncPositionEvent += OnPlayerSyncPosition;
		PlayerBeingAttackedHandler.playersBeingAttackedEvent += OnPlayersBeingAttacked;
	}
	private void OnPlayersBeingAttacked(List<string> playersBeingAttacked, string attackerName)
	{
		PlayersMap[attackerName].OnServerAttack();
		foreach (var playerName in playersBeingAttacked)
		{
			PlayersMap[playerName].OnBeingAttacked();
		}
	}
	private void OnPlayerAttack(string victimName, Vector3 attackPosition, int clientTick)
	{
		SocketRequest.getInstance().SendPlayerAttackData(victimName, attackPosition, clientTick);
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
		bool isMyPlayer = playerSpawnData.playerName == GameManager.getInstance().MyPlayer.PlayerName;
		GameObject go = Instantiate(playerPrefab);
		go.tag = "Player";
		go.name = playerSpawnData.playerName;
		ClientPlayer clientPlayer = go.GetComponent<ClientPlayer>();
		clientPlayer.Initialize(playerSpawnData, isMyPlayer);
		PlayersMap.Add(playerSpawnData.playerName, clientPlayer);
		if (isMyPlayer)
		{
			cinemachineVirtualCamera.Follow = clientPlayer.LookPoint;
		}
	}

	private void OnPlayerSyncPosition(string playerName, Vector3 position, Vector3 rotation, int time)
	{
		PlayersMap[playerName].OnServerDataUpdate(position, rotation, time);
	}

	private void OnPlayerInputChange(PlayerInputData inputData, Quaternion nextRotation)
	{
		SocketRequest.getInstance().SendPlayerInputData(inputData, nextRotation.eulerAngles);
	}
}
