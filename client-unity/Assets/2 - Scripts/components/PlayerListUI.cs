using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerListUI : MonoBehaviour
{
    [SerializeField]
    private GameObject playerButtonPrefab;

    public void UpdateRoomPlayers()
    {
        List<Player> players = RoomManager.getInstance().CurrentRoomPlayers;
        Debug.Log("PlayerList.UpdateRoomPlayers: " + string.Join(",", players));
        gameObject.GetComponent<ListUI>().RemoveAllItems();
        foreach (Player player in players)

        {
            GameObject go = gameObject.GetComponent<ListUI>().AddItem(playerButtonPrefab);
            go.GetComponentInChildren<Text>().text = player.PlayerName;
        }
    }
}
