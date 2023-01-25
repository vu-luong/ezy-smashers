using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client.logger;
using com.tvd12.ezyfoxserver.client.unity;
using UnityEngine;
using UnityEngine.UI;

public class RoomIdList : MonoBehaviour
{
	[SerializeField]
	private GameObject roomButtonPrefab;
	
	private static readonly EzyLogger LOGGER = EzyUnityLoggerFactory.getLogger<RoomIdList>();

	public void SetRoomIdList(List<int> roomIdList)
	{
		roomIdList.Sort();
		LOGGER.debug("SetRoomIdList: " + string.Join(",", roomIdList));
		gameObject.GetComponent<ListUI>().RemoveAllItems();
		foreach (int roomId in roomIdList)
		{
			GameObject go = gameObject.GetComponent<ListUI>().AddItem(roomButtonPrefab);
			go.GetComponent<ButtonUI>().Index = roomId;
			go.GetComponentInChildren<Text>().text = "Room #" + roomId;
		}
	}
}
