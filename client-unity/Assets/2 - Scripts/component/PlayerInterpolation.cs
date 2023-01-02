using UnityEngine;

public class PlayerInterpolation : MonoBehaviour
{
	private float lastInputTime;
	private ClientPlayer clientPlayer;

	public PlayerStateModel CurrentPlayerState { get; set; }
	public PlayerStateModel PreviousPlayerState { get; set; }

	private void Start()
	{
		clientPlayer = GetComponent<ClientPlayer>();
	}

	public void SetFramePosition(PlayerStateModel playerState)
	{
		RefreshToPosition(playerState, CurrentPlayerState);
	}
	private void RefreshToPosition(PlayerStateModel playerState, PlayerStateModel prevPlayerState)
	{
		PreviousPlayerState = prevPlayerState;
		CurrentPlayerState = playerState;
		lastInputTime = Time.fixedTime;
	}

	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.L))
		{
			SetFramePosition(new PlayerStateModel(new Vector3(10f, 10f, 10f), Quaternion.identity));
		}

		float timeSinceLastInput = Time.time - lastInputTime;
		float duration = clientPlayer.IsMyPlayer ? Time.fixedDeltaTime : GameConstants.SERVER_FIXED_DELTA_TIME;
		float t = timeSinceLastInput / duration;

		if (!clientPlayer.IsMyPlayer)
		{
			Vector3 movement = CurrentPlayerState.Position - transform.position;
			movement.x = movement.x != 0f ? 1f : 0f;
			movement.z = movement.z != 0f ? 1f : 0f;
			var moveInputMagnitude = new Vector2(movement.x, movement.z).sqrMagnitude;

			if (moveInputMagnitude > 0)
			{
				clientPlayer.Anim.SetFloat("Blend", moveInputMagnitude, clientPlayer.startAnimTime, Time.deltaTime);
			}
			else
			{
				clientPlayer.Anim.SetFloat("Blend", moveInputMagnitude, clientPlayer.stopAnimTime, Time.deltaTime);
			}
			
			// Debug.Log("PlayerInterpolation.Update + others + " + clientPlayer.ClientTick + ", t = " + t);
		}

		transform.position = Vector3.Lerp(PreviousPlayerState.Position, CurrentPlayerState.Position, t);
		transform.rotation = Quaternion.Lerp(PreviousPlayerState.Rotation, CurrentPlayerState.Rotation, t);
	}
}
