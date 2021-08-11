using com.tvd12.ezyfoxserver.client.util;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : EzyLoggable
{
    private static readonly GameManager INSTANCE = new GameManager();
    private Player myPlayer;

    public Player MyPlayer { get => myPlayer; }

    public GameManager()
    {
        JoinLobbyResponseHandler.joinedLobbyEvent += SetUpPlayer;
        // TODO
        // JoinRoomResponseHandler.joinedRoomEvent += setCurrentRoomId;
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
