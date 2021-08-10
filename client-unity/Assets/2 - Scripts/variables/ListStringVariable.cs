using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ListStringVariable : ScriptableObject
{
#if UNITY_EDITOR
    [Multiline]
    public string DeveloperDescription = "";
#endif

    public List<string> list;

    public void SetValue(List<string> newList)
    {
        list = new List<string>(newList);
    }
}
