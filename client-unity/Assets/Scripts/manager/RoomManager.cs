using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client.util;

public class RoomManager : EzyLoggable
{
    private static readonly RoomManager INSTANCE = new();
    private long currentRoomId;

    public long CurrentRoomId { get => currentRoomId; set => currentRoomId = value; }
    public List<PlayerModel> CurrentRoomPlayers { get; private set; }

    public static RoomManager GetInstance()
    {
        return INSTANCE;
    }

    public void SetCurrentRoomPlayers(List<string> playerNames, string master)
    {
        logger.debug("SetCurrentRoomPlayers");
        CurrentRoomPlayers = new List<PlayerModel>();
        foreach (string playerName in playerNames)
        {
            PlayerModel player;
            if (playerName.Equals(GameManager.GetInstance().MyPlayer.PlayerName))
            {
                player = GameManager.GetInstance().MyPlayer;
            } else
            { 
                player = new PlayerModel(playerName);
            }
            player.IsMaster = playerName.Equals(master);
            CurrentRoomPlayers.Add(player);
        }
    }

    public void SetCurrentRoomId(long roomId)
    {
        logger.debug("SetCurrentRoomId");
        currentRoomId = roomId;
    }

    public void ExitCurrentRoom()
    { 
        // TODO
    }
}
