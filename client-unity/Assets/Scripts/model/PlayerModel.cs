public class PlayerModel
{
    public string PlayerName { get; }
    public bool IsMaster { get; }
    
    public PlayerModel(string playerName, bool isMaster)
    {
        PlayerName = playerName;
        IsMaster = isMaster;
    }
}
