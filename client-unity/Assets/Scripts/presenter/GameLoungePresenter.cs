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
		long currentRoomId = RoomService.GetInstance().GetPlayingRoomId();
		setRoomTitleEvent?.Invoke("Room #" + currentRoomId);
	}

	public void UpdateMmoRoomPlayers(List<PlayerModel> models)
	{
		PlayerService.GetInstance().AddAllRoomPlayers(models);
		myPlayerIsMasterCheckedEvent?.Invoke(PlayerService.GetInstance().GetMyPlayer().IsMaster);
		roomPlayersUpdatedEvent?.Invoke();
	}

	public void GameStarted(List<PlayerSpawnInfoModel> models)
	{
		PlayerService.GetInstance().AddAllPlayerSpawnInfos(models);
		SceneManager.LoadScene("MainScene");
	}
}
