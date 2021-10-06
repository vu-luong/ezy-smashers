using _2___Scripts.shared;
using UnityEngine;

public class GamePlayControllers : MonoBehaviour
{
	private void Awake()
	{
		Movement.playerInputEvent += OnPlayerInputChange;
	}
	private void OnPlayerInputChange(PlayerInputData inputData)
	{
		SocketRequest.getInstance().SendPlayerInputData(inputData);
	}
}
