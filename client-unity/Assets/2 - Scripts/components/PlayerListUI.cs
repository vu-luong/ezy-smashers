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
			string displayName = player.IsMaster ?
				player.PlayerName + "(Master)" : player.PlayerName;
			go.GetComponentInChildren<Text>().text = displayName;
		}
	}
}
