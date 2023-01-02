using UnityEngine;

public class StartGameButtonUI : MonoBehaviour
{
    public void UpdatePlayersResponse() {
        gameObject.SetActive(GameManager.getInstance().MyPlayer.IsMaster);
    }
}
