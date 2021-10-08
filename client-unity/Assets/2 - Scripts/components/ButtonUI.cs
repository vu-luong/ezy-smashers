using UnityEngine;
using UnityEngine.Events;

public class ButtonUI : MonoBehaviour
{
    public UnityEvent<int> onClickEvent;
    private int index = -1;

    public int Index { get => index; set => index = value; }

    public int GetIndex()
    {
        if (index == -1) 
        { 
            return transform.GetSiblingIndex();
        } else
        {
            return index;
        }
    }

    public void OnClick()
    {
        Debug.Log("Item " + GetIndex());
        onClickEvent.Invoke(GetIndex());
    }
}
