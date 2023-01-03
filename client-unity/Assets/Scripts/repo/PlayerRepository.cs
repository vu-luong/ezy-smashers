using System.Collections.Generic;

public class PlayerRepository
{
	private static readonly PlayerRepository INSTANCE = new();
	private readonly Dictionary<string, ClientPlayer> playerByName = new();
	private readonly PlayerEntity myPlayer = new();
	private readonly List<PlayerEntity> currentRoomPlayers = new();
	private readonly List<PlayerSpawnEntity> playerSpawnInfos = new();

	public static PlayerRepository GetInstance()
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

	public void ClearPlayerByName()
	{
		playerByName.Clear();
	}

	public void ClearPlayerSpawnInfos()
	{
		playerSpawnInfos.Clear();
	}

	public void UpdateMyPlayer(PlayerModel model)
	{
		myPlayer.PlayerName = model.PlayerName;
		myPlayer.IsMaster = model.IsMaster;
	}

	public string GetMyPlayerName()
	{
		return myPlayer.PlayerName;
	}

	public PlayerModel GetMyPlayer()
	{
		return new PlayerModel(myPlayer.PlayerName, myPlayer.IsMaster);
	}
	
	public void UpdateRoomPlayers(List<PlayerModel> models)
	{
		currentRoomPlayers.Clear();
		foreach (PlayerModel model in models)
		{
			PlayerEntity entity;
			if (model.PlayerName.Equals(myPlayer.PlayerName))
			{
				entity = myPlayer;
			} else
			{ 
				entity = new PlayerEntity();
				entity.PlayerName = model.PlayerName;
			}
			entity.IsMaster = model.IsMaster;
			currentRoomPlayers.Add(entity);
		}
	}
	
	public List<PlayerModel> GetRoomPlayers()
	{
		List<PlayerModel> models = new();
		foreach (PlayerEntity entity in currentRoomPlayers)
		{
			models.Add(new PlayerModel(entity.PlayerName, entity.IsMaster));
		}
		return models;
	}
	
	public void UpdatePlayerSpawnInfos(List<PlayerSpawnInfoModel> models)
	{
		foreach (PlayerSpawnInfoModel model in models)
		{
			PlayerSpawnEntity entity = new PlayerSpawnEntity();
			entity.PlayerName = model.PlayerName;
			entity.Position = model.Position;
			entity.PlayerColor = model.PlayerColor;
			playerSpawnInfos.Add(entity);
		}
	}

	public List<PlayerSpawnInfoModel> GetPlayerSpawnInfos()
	{
		List<PlayerSpawnInfoModel> models = new();
		foreach (PlayerSpawnEntity entity in playerSpawnInfos)
		{
			models.Add(new PlayerSpawnInfoModel(entity.PlayerName, entity.Position, entity.PlayerColor));
		}
		return models;
	}
}
