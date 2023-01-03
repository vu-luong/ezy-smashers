using Cinemachine;
using UnityEngine;

public class PlayerFollowingCamera : MonoBehaviour
{
	public CinemachineVirtualCamera cinemachineVirtualCamera;
	
	public void SetFollow(ClientPlayer clientPlayer)
	{
		Debug.Log("SetFollow myClientPlayer.lookPoint = " + clientPlayer.LookPoint);
		cinemachineVirtualCamera.Follow = clientPlayer.LookPoint;
	}
}
