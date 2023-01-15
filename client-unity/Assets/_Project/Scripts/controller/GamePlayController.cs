using com.tvd12.ezyfoxserver.client.entity;
using com.tvd12.ezyfoxserver.client.support;
using com.tvd12.ezyfoxserver.client.unity;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GamePlayController : EzyDefaultController
{
	[SerializeField]
	private UnityEvent<PlayerSyncPositionModel> playerSyncPositionEvent;
	
	[SerializeField]
	private UnityEvent<string> playerBeingAttackedEvent;
	
	[SerializeField]
	private UnityEvent<string> otherPlayerAttackEvent;
	
	private void Awake()
	{
		base.Awake();
		addHandler<EzyArray>(Commands.SYNC_POSITION, OnPlayerSyncPosition);
		addHandler<EzyObject>(Commands.PLAYER_BEING_ATTACKED, OnPlayerBeingAttacked);
		addHandler<EzyObject>(Commands.PLAYER_ATTACK_DATA, OnPlayerAttackResponse);
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
		playerSyncPositionEvent?.Invoke(new PlayerSyncPositionModel(playerName, position, rotation, time));
	}
	
	private void OnPlayerBeingAttacked(EzyAppProxy proxy, EzyObject data)
	{
		var victimName = data.get<string>("v");
		var attackTime = data.get<float>("t");
		var attackerName = data.get<string>("a");
		var attackPosition = data.get<EzyArray>("p");
		logger.debug(
			"victimName: " + victimName + "; attackTime: " + attackTime +
			"; attackerName: " + attackerName + "; attackPosition: " + attackPosition
		);
		playerBeingAttackedEvent?.Invoke(victimName);
	}
	
	private void OnPlayerAttackResponse(EzyAppProxy proxy, EzyObject data)
	{
		var attackerName = data.get<string>("a");
		logger.debug("OnPlayerAttackResponse - attackerName = " + attackerName);
		otherPlayerAttackEvent?.Invoke(attackerName);
	}
	
	#region Public Methods
	
	public void OnPlayerAttack(Vector3 attackPosition, int clientTick)
	{
		SocketRequest.GetInstance().SendPlayerAttackData(attackPosition, clientTick);
	}
	
	public void OnPlayerInputChange(PlayerInputModel playerInput, Quaternion nextRotation)
	{
		SocketRequest.GetInstance().SendPlayerInputData(playerInput, nextRotation.eulerAngles);
	}
	
	public void OnPlayerHit(PlayerHitModel playerHit)
	{
		string victimName = playerHit.VictimName;
		Vector3 attackPosition = playerHit.AttackPosition;
		int myClientTick = playerHit.AttackerTick;
		int otherClientTick = playerHit.VictimTick;
		// todo vu: should convert to PlayerHitRequest
		SocketRequest.GetInstance().SendPlayerHit(victimName, attackPosition, myClientTick, otherClientTick);
	}

	public void ExitGameRoom()
	{
		SceneManager.LoadScene("LobbyScene");
	}

	#endregion
}
