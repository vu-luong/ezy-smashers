using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameLoungePresenter : MonoBehaviour
{
	[SerializeField]
	private UnityEvent<string> setRoomTitleEvent;
	
	[SerializeField]
	private UnityEvent roomPlayersUpdatedEvent;

	[SerializeField]
	private UnityEvent<bool> myPlayerIsMasterCheckedEvent;

	private void Awake()
	{
		SetRoomTitle();	
	}
	
	private void SetRoomTitle()
	{
		long currentRoomId = RoomRepository.GetInstance().GetPlayingRoomId();
		setRoomTitleEvent?.Invoke("Room #" + currentRoomId);
	}

	public void UpdateMmoRoomPlayers(List<PlayerModel> models)
	{
		PlayerRepository.GetInstance().UpdateRoomPlayers(models);
		myPlayerIsMasterCheckedEvent?.Invoke(PlayerRepository.GetInstance().GetMyPlayer().IsMaster);
		roomPlayersUpdatedEvent?.Invoke();
	}

	public void GameStarted(List<PlayerSpawnInfoModel> models)
	{
		PlayerRepository.GetInstance().UpdatePlayerSpawnInfos(models);
		SceneManager.LoadScene("MainScene");
	}
}
