using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerListUI : MonoBehaviour
{
	[SerializeField]
	private GameObject playerButtonPrefab;

	public void UpdateRoomPlayers(List<PlayerModel> models)
	{
		Debug.Log("PlayerList.UpdateRoomPlayers: " + string.Join(",", models));
		gameObject.GetComponent<ListUI>().RemoveAllItems();
		foreach (PlayerModel player in models)
		{
			GameObject go = gameObject.GetComponent<ListUI>().AddItem(playerButtonPrefab);
			string displayName = player.IsMaster ? player.PlayerName + "(Master)" : player.PlayerName;
			go.GetComponentInChildren<Text>().text = displayName;
		}
	}
}
