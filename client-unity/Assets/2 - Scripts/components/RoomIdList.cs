using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomIdList : MonoBehaviour
{
	[SerializeField]
	private GameObject roomButtonPrefab;

	public void SetRoomIdList(List<int> roomIdList)
	{
		roomIdList.Sort();
		Debug.Log("RoomIdList.SetRoomIdList: " + string.Join(",", roomIdList));
		gameObject.GetComponent<ListUI>().RemoveAllItems();
		foreach (int roomId in roomIdList)

		{
			GameObject go = gameObject.GetComponent<ListUI>().AddItem(roomButtonPrefab);
			go.GetComponent<ButtonUI>().Index = roomId;
			go.GetComponentInChildren<Text>().text = "Room #" + roomId;
		}
	}
}
