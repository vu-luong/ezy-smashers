using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomNames : MonoBehaviour
{
    [SerializeField]
    private GameObject roomButtonPrefab;

    public void SetRoomNames(List<string> roomNames)
    {
        Debug.Log("RoomNames.SetRoomNames: " + string.Join(",", roomNames));
        gameObject.GetComponent<ListUI>().RemoveAllItems();
        foreach (string roomName in roomNames)
        {
            GameObject go = gameObject.GetComponent<ListUI>().AddItem(roomButtonPrefab);
            go.GetComponentInChildren<Text>().text = roomName;
        }
    }
}
