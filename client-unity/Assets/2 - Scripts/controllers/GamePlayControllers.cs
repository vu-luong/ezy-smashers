using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GamePlayControllers : MonoBehaviour
{
	public GameObject playerPrefab;
	public CinemachineVirtualCamera cinemachineVirtualCamera;
	[FormerlySerializedAs("gameOverTextChange")]
	public UnityEvent gameOverUIUpdateEvent;

	private Dictionary<string, ClientPlayer> playersMap = new Dictionary<string, ClientPlayer>();
	private ClientPlayer myPlayer;

	public Dictionary<string, ClientPlayer> PlayersMap => playersMap;

	private void Awake()
	{
		SpawnPlayers(GameManager.getInstance().PlayersSpawnData);
		SyncPositionHandler.action += OnPlayerSyncPosition;
		PlayerBeingAttackedHandler.action += OnPlayersBeingAttacked;
		PlayerAttackDataHandler.action += OnPlayerAttackResponse;
	}

	private void UnregisterEvents()
	{
		myPlayer.PlayerInputEvent -= OnPlayerInputChange;
		myPlayer.PlayerAttackEvent -= OnPlayerAttack;
		myPlayer.GameOverEvent -= OnGameOver;
		myPlayer.Hammer1.PlayerHitEvent -= OnPlayerHit;
		SyncPositionHandler.action -= OnPlayerSyncPosition;
		PlayerBeingAttackedHandler.action -= OnPlayersBeingAttacked;
		PlayerAttackDataHandler.action -= OnPlayerAttackResponse;
	}

	private void OnGameOver()
	{
		Debug.Log("OnGameOver");
		gameOverUIUpdateEvent?.Invoke();
	}

	private void OnPlayerAttackResponse(string attackerName)
	{
		PlayersMap[attackerName].OnServerAttack();
	}

	private void OnPlayerAttack(Vector3 attackPosition, int clientTick)
	{
		SocketRequest.getInstance().SendPlayerAttackData(attackPosition, clientTick);
	}

	private void OnPlayersBeingAttacked(string victimName, string attackerName)
	{
		Debug.Log("playerBeingAttacked " + victimName);
		PlayersMap[victimName].OnBeingAttacked();
	}

	private void OnPlayerHit(string victimName, Vector3 attackPosition, int myClientTick, int otherClientTick)
	{
		SocketRequest.getInstance().SendPlayerHit(victimName, attackPosition, myClientTick, otherClientTick);
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
			myPlayer = clientPlayer;
			myPlayer.PlayerInputEvent += OnPlayerInputChange;
			myPlayer.PlayerAttackEvent += OnPlayerAttack;
			myPlayer.GameOverEvent += OnGameOver;
			myPlayer.Hammer1.PlayerHitEvent += OnPlayerHit;
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

	#region Public Methods

	public void OnExitGameRoom()
	{
		UnregisterEvents();
		SceneManager.LoadScene("LobbyScene");
	}

	#endregion
}
