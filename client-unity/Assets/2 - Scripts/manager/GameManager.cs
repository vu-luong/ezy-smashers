using System;
using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client.util;

public class GameManager : EzyLoggable
{
	private static readonly GameManager INSTANCE = new();
	private PlayerModel myPlayer;
	
	public PlayerModel MyPlayer => myPlayer;
	public List<PlayerSpawnInfoModel> PlayersSpawnInfo { get; set; }

	public static GameManager getInstance()
	{
		return INSTANCE;
	}

	public void SetUpMyPlayer(String username)
	{
		logger.debug("Setup my player");
		myPlayer = new PlayerModel(username);
	}
}
