using System.Collections.Generic;
using UnityEngine;

public abstract class GameEvent : ScriptableObject
{
    private readonly List<GameEventListener> eventListeners = new();

    public void Raise()
    {
        for (int i = eventListeners.Count - 1; i >= 0; i--)
        {
            eventListeners[i].OnEventRaised();
        }
    }

    public void RegisterListener(GameEventListener listener)
    {
        if (!eventListeners.Contains(listener))
        {
            eventListeners.Add(listener);
        }
    }

    public void UnregisterListener(GameEventListener listener)
    {
        if (eventListeners.Contains(listener))
        {
            eventListeners.Remove(listener);
        }
    }
}

public abstract class GameEvent<T> : ScriptableObject
{
    private readonly List<GameEventListener<T>> eventListeners = new();

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

public abstract class GameEvent<T0, T1> : ScriptableObject
{
    private readonly List<GameEventListener<T0, T1>> eventListeners = new();

    public void Raise(T0 item0, T1 item1)
    {
        for (int i = eventListeners.Count - 1; i >= 0; i--)
        {
            eventListeners[i].OnEventRaised(item0, item1);
        }
    }

    public void RegisterListener(GameEventListener<T0, T1> listener)
    {
        if (!eventListeners.Contains(listener))
        {
            eventListeners.Add(listener);
        }
    }

    public void UnregisterListener(GameEventListener<T0, T1> listener)
    {
        if (eventListeners.Contains(listener))
        {
            eventListeners.Remove(listener);
        }
    }
}
