using System.Collections.Generic;
using Cinemachine;
using com.tvd12.ezyfoxserver.client.entity;
using com.tvd12.ezyfoxserver.client.support;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GamePlayController : DefaultMonoBehaviour
{
	public GameObject playerPrefab;
	public CinemachineVirtualCamera cinemachineVirtualCamera;
	[FormerlySerializedAs("gameOverTextChange")]
	public UnityEvent gameOverUIUpdateEvent;

	private ClientPlayer myPlayer;
	private Dictionary<string, ClientPlayer> playerByName = new();
	private Dictionary<string, ClientPlayer> PlayerByName => playerByName;

	private void Awake()
	{
		AddHandler<EzyArray>(Commands.SYNC_POSITION, OnPlayerSyncPosition);
		AddHandler<EzyObject>(Commands.PLAYER_BEING_ATTACKED, OnPlayersBeingAttacked);
		AddHandler<EzyObject>(Commands.PLAYER_ATTACK_DATA, OnPlayerAttackResponse);
		SpawnPlayers(GameManager.getInstance().PlayersSpawnData);
	}

	private void OnPlayerSyncPosition(EzyAppProxy proxy, EzyArray data)
	{
		logger.info("OnPlayerSyncPosition: " + data);
		string playerName = data.get<string>(0);
		EzyArray positionArray = data.get<EzyArray>(1);
		EzyArray rotationArray = data.get<EzyArray>(2);
		int time = data.get<int>(3);
		Vector3 position = new Vector3(
			positionArray.get<float>(0),
			positionArray.get<float>(1),
			positionArray.get<float>(2)
		);
		Vector3 rotation = new Vector3(
			rotationArray.get<float>(0),
			rotationArray.get<float>(1),
			rotationArray.get<float>(2)
		);
		PlayerByName[playerName].OnServerDataUpdate(position, rotation, time);
	}
	
	private void OnPlayersBeingAttacked(EzyAppProxy proxy, EzyObject data)
	{
		var victimName = data.get<string>("v");
		var attackTime = data.get<float>("t");
		var attackerName = data.get<string>("a");
		var attackPosition = data.get<EzyArray>("p");
		logger.info(
			"victimName: " + victimName + "; attackTime: " + attackTime +
			"; attackerName: " + attackerName + "; attackPosition: " + attackPosition
		);
		PlayerByName[victimName].OnBeingAttacked();
	}
	
	private void OnPlayerAttackResponse(EzyAppProxy proxy, EzyObject data)
	{
		var attackerName = data.get<string>("a");
		logger.debug("OnPlayerAttackResponse - attackerName = " + attackerName);
		PlayerByName[attackerName].OnServerAttack();
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
		PlayerByName.Add(playerSpawnData.playerName, clientPlayer);
		if (isMyPlayer)
		{
			myPlayer = clientPlayer;
			// todo vu: refactor this, this is highly coupling with ClientPlayer
			// myPlayer.playerInputEvent += OnPlayerInputChange;
			// myPlayer.PlayerAttackEvent += OnPlayerAttack;
			// myPlayer.gameOverEvent += OnGameOver;
			myPlayer.Hammer1.PlayerHitEvent += OnPlayerHit;
			cinemachineVirtualCamera.Follow = clientPlayer.LookPoint;
		}
	}
	
	private void OnPlayerHit(string victimName, Vector3 attackPosition, int myClientTick, int otherClientTick)
	{
		SocketRequest.getInstance().SendPlayerHit(victimName, attackPosition, myClientTick, otherClientTick);
	}
	
	private void UnregisterEvents()
	{
		// myPlayer.playerInputEvent -= OnPlayerInputChange;
		// myPlayer.PlayerAttackEvent -= OnPlayerAttack;
		// myPlayer.gameOverEvent -= OnGameOver;
		myPlayer.Hammer1.PlayerHitEvent -= OnPlayerHit;
	}

	#region Public Methods
	
	public void OnPlayerAttack(Vector3 attackPosition, int clientTick)
	{
		SocketRequest.getInstance().SendPlayerAttackData(attackPosition, clientTick);
	}
	
	public void OnPlayerInputChange(PlayerInputData inputData, Quaternion nextRotation)
	{
		SocketRequest.getInstance().SendPlayerInputData(inputData, nextRotation.eulerAngles);
	}
	
	public void OnMyPlayerDead()
	{
		Debug.Log("OnMyPlayerDead");
		gameOverUIUpdateEvent?.Invoke();
	}

	public void ExitGameRoom()
	{
		UnregisterEvents();
		SceneManager.LoadScene("LobbyScene");
	}

	#endregion
}
