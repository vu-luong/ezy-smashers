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

	private readonly Dictionary<string, ClientPlayer> playerByName = new();

	private void Start()
	{
		AddHandler<EzyArray>(Commands.SYNC_POSITION, OnPlayerSyncPosition);
		AddHandler<EzyObject>(Commands.PLAYER_BEING_ATTACKED, OnPlayersBeingAttacked);
		AddHandler<EzyObject>(Commands.PLAYER_ATTACK_DATA, OnPlayerAttackResponse);
		SpawnPlayers(GameManager.getInstance().PlayersSpawnInfo);
	}

	private void OnPlayerSyncPosition(EzyAppProxy proxy, EzyArray data)
	{
		logger.debug("OnPlayerSyncPosition: " + data);
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
		playerByName[playerName].OnServerDataUpdate(position, rotation, time);
	}
	
	private void OnPlayersBeingAttacked(EzyAppProxy proxy, EzyObject data)
	{
		var victimName = data.get<string>("v");
		var attackTime = data.get<float>("t");
		var attackerName = data.get<string>("a");
		var attackPosition = data.get<EzyArray>("p");
		logger.debug(
			"victimName: " + victimName + "; attackTime: " + attackTime +
			"; attackerName: " + attackerName + "; attackPosition: " + attackPosition
		);
		playerByName[victimName].OnBeingAttacked();
	}
	
	private void OnPlayerAttackResponse(EzyAppProxy proxy, EzyObject data)
	{
		var attackerName = data.get<string>("a");
		logger.debug("OnPlayerAttackResponse - attackerName = " + attackerName);
		playerByName[attackerName].OnServerAttack();
	}
	
	private void SpawnPlayers(List<PlayerSpawnInfoModel> playerSpawnInfos)
	{
		foreach (var playerSpawnData in playerSpawnInfos)
		{
			SpawnPlayer(playerSpawnData);
		}
	}
	
	private void SpawnPlayer(PlayerSpawnInfoModel playerSpawnData)
	{
		bool isMyPlayer = playerSpawnData.PlayerName == GameManager.getInstance().MyPlayer.PlayerName;
		GameObject go = Instantiate(playerPrefab);
		go.tag = "Player";
		go.name = playerSpawnData.PlayerName;
		ClientPlayer clientPlayer = go.GetComponent<ClientPlayer>();
		clientPlayer.Initialize(playerSpawnData, isMyPlayer);
		playerByName.Add(playerSpawnData.PlayerName, clientPlayer);
		if (isMyPlayer)
		{
			cinemachineVirtualCamera.Follow = clientPlayer.LookPoint;
		}
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
	
	public void OnPlayerHit(PlayerHitModel playerHit)
	{
		string victimName = playerHit.VictimName;
		Vector3 attackPosition = playerHit.AttackPosition;
		int myClientTick = playerHit.AttackerTick;
		int otherClientTick = playerHit.VictimTick;
		// todo vu: should convert to PlayerHitRequest
		SocketRequest.getInstance().SendPlayerHit(victimName, attackPosition, myClientTick, otherClientTick);
	}

	public void ExitGameRoom()
	{
		SceneManager.LoadScene("LobbyScene");
	}

	#endregion
}
