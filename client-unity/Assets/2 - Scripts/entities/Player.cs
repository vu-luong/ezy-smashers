public class Player
{
    private string playerName;
    private bool isMaster;

    public Player(string playerName)
    {
        this.playerName = playerName;
    }

    public string PlayerName { get => playerName; set => playerName = value; }
    public bool IsMaster { get => isMaster; set => isMaster = value; }
}
