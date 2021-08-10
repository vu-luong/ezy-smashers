using UnityEngine;

public class GameLoungeController : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log("PlayerName: " + GameManager.getInstance().Player.PlayerName);
        Debug.Log("IsMaster: " + GameManager.getInstance().Player.IsMaster);
        Debug.Log("Current room id: " + GameManager.getInstance().Player.CurrentRoomId);
    }
}
