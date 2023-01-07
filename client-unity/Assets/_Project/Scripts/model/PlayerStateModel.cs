using UnityEngine;

public class PlayerStateModel
{
	public Vector3 Position { get; set; }
	public Quaternion Rotation { get; set; }
	
	public PlayerStateModel(Vector3 position, Quaternion rotation)
	{
		Position = position;
		Rotation = rotation;
	}
}
