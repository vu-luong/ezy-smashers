using System.Collections.Generic;
using UnityEngine;

public abstract class GameEvent<T> : ScriptableObject
{
    private readonly List<GameEventListener<T>> eventListeners =
        new List<GameEventListener<T>>();

    public void Raise(T item)
    {
        for (int i = eventListeners.Count - 1; i >= 0; i--)
        {
            eventListeners[i].OnEventRaised(item);
        }
    }

    public void RegisterListener(GameEventListener<T> listener)
    {
        if (!eventListeners.Contains(listener))
        {
            eventListeners.Add(listener);
        }
    }

    public void UnregisterListener(GameEventListener<T> listener)
    {
        if (eventListeners.Contains(listener))
        {
            eventListeners.Remove(listener);
        }
    }
}
