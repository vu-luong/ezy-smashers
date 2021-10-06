using _2___Scripts.shared;
using UnityEngine;

public class PlayerInterpolation : MonoBehaviour
{
	private float lastInputTime;

	public PlayerStateData CurrentData { get; set; }
	public PlayerStateData PreviousData { get; set; }

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
		float timeSinceLastInput = Time.time - lastInputTime;
		float t = timeSinceLastInput / Time.fixedDeltaTime;
		transform.position = Vector3.Lerp(PreviousData.Position, CurrentData.Position, t);
		transform.rotation = Quaternion.Lerp(PreviousData.Rotation, CurrentData.Rotation, t);
	}
}
