using com.tvd12.ezyfoxserver.client.util;
using UnityEngine;

public class GameManager : EzyLoggable
{
    private static readonly GameManager INSTANCE = new GameManager();
    private Player player;

    public Player Player { get => player; }

    public GameManager()
    {
        JoinLobbyResponseHandler.joinedLobbyEvent += setupPlayer;
        CreateRoomResponseHandler.roomCreatedEvent += setCurrentRoomId;
        // TODO
        // JoinRoomResponseHandler.joinedRoomEvent += setCurrentRoomId;
    }

    public static GameManager getInstance()
    {
        return INSTANCE;
    }

    public void setupPlayer() 
    {
        Debug.Log("GameManager.setupPlayer");
        player = new Player(SocketProxy.getInstance().UserAuthenInfo.Username);
    }

    public void setCurrentRoomId(long roomId, bool master)
    {
        Debug.Log("GameManager.setCurrentRoomId");
        player.CurrentRoomId = roomId;
        player.IsMaster = master;
    }
}
