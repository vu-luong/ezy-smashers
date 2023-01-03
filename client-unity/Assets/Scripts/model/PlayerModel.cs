public class PlayerModel
{
    public string PlayerName { get; }
    public bool IsMaster { get; set; }
    
    public PlayerModel(string playerName)
    {
        PlayerName = playerName;
    }
}
