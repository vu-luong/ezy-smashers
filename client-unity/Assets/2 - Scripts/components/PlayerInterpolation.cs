using UnityEngine;

public class PlayerInterpolation : MonoBehaviour
{
	private float lastInputTime;
	private ClientPlayer clientPlayer;

	public PlayerStateData CurrentData { get; set; }
	public PlayerStateData PreviousData { get; set; }

	private void Start()
	{
		clientPlayer = GetComponent<ClientPlayer>();
	}

	public void SetFramePosition(PlayerStateData data)
	{
		RefreshToPosition(data, CurrentData);
	}
	private void RefreshToPosition(PlayerStateData data, PlayerStateData prevData)
	{
		PreviousData = prevData;
		CurrentData = data;
		lastInputTime = Time.fixedTime;
	}

	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.L))
		{
			SetFramePosition(new PlayerStateData(new Vector3(10f, 10f, 10f), Quaternion.identity));
		}

		float timeSinceLastInput = Time.time - lastInputTime;
		float duration = clientPlayer.IsMyPlayer ? Time.fixedDeltaTime : SocketConstants.SERVER_FIXED_DELTA_TIME;
		float t = timeSinceLastInput / duration;

		if (!clientPlayer.IsMyPlayer)
		{
			Vector3 movement = CurrentData.Position - transform.position;
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

		transform.position = Vector3.Lerp(PreviousData.Position, CurrentData.Position, t);
		transform.rotation = Quaternion.Lerp(PreviousData.Rotation, CurrentData.Rotation, t);
	}
}
