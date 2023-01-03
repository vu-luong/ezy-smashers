using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerSpawnerPresenter : MonoBehaviour
{
	public GameObject playerPrefab;
	[SerializeField]
	private UnityEvent<ClientPlayer> myPlayerSpawnedEvent;

	public void Start()
	{
		// todo vu: get from GameService
		SpawnPlayers(GameManager.GetInstance().PlayersSpawnInfo);
	}
	
	private void SpawnPlayers(List<PlayerSpawnInfoModel> playerSpawnInfos)
	{
		foreach (var playerSpawnData in playerSpawnInfos)
		{
			SpawnPlayer(playerSpawnData);
		}
	}
	
	private void SpawnPlayer(PlayerSpawnInfoModel playerSpawnInfo)
	{
		bool isMyPlayer = playerSpawnInfo.PlayerName == GameManager.GetInstance().MyPlayer.PlayerName;
		GameObject go = Instantiate(playerPrefab);
		go.tag = "Player";
		go.name = playerSpawnInfo.PlayerName;
		ClientPlayer clientPlayer = go.GetComponent<ClientPlayer>();
		clientPlayer.Initialize(playerSpawnInfo, isMyPlayer);
		PlayerService.GetInstance().AddPlayer(playerSpawnInfo.PlayerName, clientPlayer);
		if (isMyPlayer)
		{
			Debug.Log("myClientPlayer.lookPoint = " + clientPlayer.LookPoint);
			myPlayerSpawnedEvent?.Invoke(clientPlayer);
		}
	}
}
