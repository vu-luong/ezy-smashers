using UnityEngine;

public class PlayerSpawnData
{
	public string playerName;
	public Vector3 position;
	public Vector3 color;

	public PlayerSpawnData(string playerName, Vector3 position, Vector3 color)
	{
		this.playerName = playerName;
		this.position = position;
		this.color = color;
	}

	public override string ToString()
	{
		return $"{playerName},{position.ToString("F4")},{color}";
	}
}
