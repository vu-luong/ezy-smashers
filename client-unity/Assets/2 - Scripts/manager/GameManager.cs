using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client.util;
using UnityEngine;

public class GameManager : EzyLoggable
{
	private static readonly GameManager INSTANCE = new GameManager();
	private Player myPlayer;

	public Player MyPlayer { get => myPlayer; }

	public List<PlayerSpawnData> PlayersSpawnData { get; set; }

	public GameManager()
	{
		JoinLobbyResponseHandler.joinedLobbyEvent += SetUpPlayer;
	}

	public static GameManager getInstance()
	{
		return INSTANCE;
	}

	public void SetUpPlayer()
	{
		Debug.Log("GameManager.setupPlayer");
		myPlayer = new Player(SocketProxy.getInstance().UserAuthenInfo.Username);
	}
}
