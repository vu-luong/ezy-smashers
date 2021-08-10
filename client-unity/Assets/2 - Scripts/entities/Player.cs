public class Player
{
    private long currentRoomId;
    private string playerName;
    private bool isMaster;

    public Player(string playerName)
    {
        this.playerName = playerName;
    }

    public long CurrentRoomId { get => currentRoomId; set => currentRoomId = value; }
    public string PlayerName { get => playerName; set => playerName = value; }
    public bool IsMaster { get => isMaster; set => isMaster = value; }
}
