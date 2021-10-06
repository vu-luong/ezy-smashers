using _2___Scripts.shared;
using UnityEngine;

public class GamePlayControllers : MonoBehaviour
{
	private void Awake()
	{
		ClientPlayer.playerInputEvent += OnPlayerInputChange;
	}
	private void OnPlayerInputChange(PlayerInputData inputData)
	{
		SocketRequest.getInstance().SendPlayerInputData(inputData);
	}
}
