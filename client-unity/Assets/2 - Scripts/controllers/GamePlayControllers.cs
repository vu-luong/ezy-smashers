using _2___Scripts.shared;
using UnityEngine;

public class GamePlayControllers : MonoBehaviour
{

	public ClientPlayer mePlayer;
	
	private void Awake()
	{
		ClientPlayer.playerInputEvent += OnPlayerInputChange;
		SyncPositionHandler.syncPositionEvent += OnPlayerSyncPosition;
	}
	private void OnPlayerSyncPosition(string playerName, Vector3 position, Vector3 rotation, int time)
	{
		if (playerName == GameManager.getInstance().MyPlayer.PlayerName)
		{
			mePlayer.OnServerDataUpdate(position, time);
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
