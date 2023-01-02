using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerSpawnerPresenter : MonoBehaviour
{
	public GameObject playerPrefab;
	public CinemachineVirtualCamera cinemachineVirtualCamera;

	public void Awake()
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
			cinemachineVirtualCamera.Follow = clientPlayer.LookPoint;
		}
	}
}
