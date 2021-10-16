using UnityEngine;

public class PlayerStateData
{

	public PlayerStateData(Vector3 position, Quaternion rotation)
	{
		Position = position;
		Rotation = rotation;
	}

	public Vector3 Position { get; set; }

	public Quaternion Rotation { get; set; }
}
