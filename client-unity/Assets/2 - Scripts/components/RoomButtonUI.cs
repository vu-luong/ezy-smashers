using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomButtonUI : MonoBehaviour
{
    private void Start()
    {
        int currentRoomIndex = GetIndex() + 1;
    }

    public int GetIndex()
    {
        return transform.GetSiblingIndex();
    }

    public void OnClick()
    {
        Debug.Log("Room " + GetIndex());
    }
}
