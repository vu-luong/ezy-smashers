using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameNoArgsEvent : ScriptableObject
{
    private readonly List<GameNoArgsEventListener> eventListeners =
        new List<GameNoArgsEventListener>();

    public void Raise()
    {
        for (int i = eventListeners.Count - 1; i >= 0; i--)
        {
            eventListeners[i].OnEventRaised();
        }
    }

    public void RegisterListener(GameNoArgsEventListener listener)
    {
        if (!eventListeners.Contains(listener))
        {
            eventListeners.Add(listener);
        }
    }

    public void UnregisterListener(GameNoArgsEventListener listener)
    {
        if (eventListeners.Contains(listener))
        {
            eventListeners.Remove(listener);
        }
    }
}
