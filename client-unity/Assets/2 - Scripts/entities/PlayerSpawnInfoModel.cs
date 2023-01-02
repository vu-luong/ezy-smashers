using UnityEngine;

public class PlayerSpawnInfoModel
{
	public string PlayerName { get; }
	public Vector3 Position { get; }
	public Vector3 PlayerColor { get; }

	public PlayerSpawnInfoModel(string playerName, Vector3 position, Vector3 playerColor)
	{
		PlayerName = playerName;
		Position = position;
		PlayerColor = playerColor;
	}

	public override string ToString()
	{
		return $"{PlayerName},{Position.ToString("F4")},{PlayerColor}";
	}
}
