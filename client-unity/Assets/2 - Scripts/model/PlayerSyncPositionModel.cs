using UnityEngine;

public class PlayerSyncPositionModel
{
	public string PlayerName { get; }
	public Vector3 Position { get; }
	public Vector3 Rotation { get; }
	public int Time { get; }

	public PlayerSyncPositionModel(string playerName, Vector3 position, Vector3 rotation, int time)
	{
		PlayerName = playerName;
		Position = position;
		Rotation = rotation;
		Time = time;
	}
}
