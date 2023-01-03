using System.Collections.Generic;

public class PlayerService
{
	private static readonly PlayerService INSTANCE = new();
	private readonly Dictionary<string, ClientPlayer> playerByName = new();

	public static PlayerService GetInstance()
	{
		return INSTANCE;
	}
	
	public void AddPlayer(string playerName, ClientPlayer player)
	{
		playerByName.Add(playerName, player);
	}

	public ClientPlayer GetPlayerByName(string playerName)
	{
		return playerByName[playerName];
	}
}
