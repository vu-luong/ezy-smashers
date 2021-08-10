using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomList : MonoBehaviour
{
    [SerializeField]
    private GameObject roomButtonPrefab;
    //private List<string> roomNames;

    public void SetRoomNames(List<string> roomNames)
    {
        Debug.Log("RoomList.SetRoomNames: " + string.Join(",", roomNames));
        //this.roomNames = new List<string>(roomNames);
        gameObject.GetComponent<ListUI>().RemoveAllItems();
        foreach (string roomName in roomNames)
        {
            //roomButtonPrefab.GetComponentInChildren<Text>().text = roomName;
            GameObject go = gameObject.GetComponent<ListUI>().AddItem(roomButtonPrefab);
            go.GetComponentInChildren<Text>().text = roomName;
        }
    }
}
