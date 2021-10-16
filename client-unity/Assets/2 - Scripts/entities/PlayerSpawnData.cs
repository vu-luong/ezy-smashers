using UnityEngine;

public class PlayerSpawnData
{
	public string playerName;
	public Vector3 position;

	public PlayerSpawnData(string playerName, Vector3 position)
	{
		this.playerName = playerName;
		this.position = position;
	}

	public override string ToString()
	{
		return $"{playerName},{position.ToString("F4")}";
	}
}
