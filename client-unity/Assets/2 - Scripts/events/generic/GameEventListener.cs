using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    [Tooltip("Event to register with.")]
    public GameEvent Event;

    [Tooltip("Response to invoke when Event is raised")]
    public UnityEvent Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised()
    {
        Response.Invoke();
    }
}

public class GameEventListener<T> : MonoBehaviour
{
    [Tooltip("Event to register with.")]
    public GameEvent<T> Event;

    [Tooltip("Response to invoke when Event is raised")]
    public UnityEvent<T> Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(T item)
    {
        Response.Invoke(item);
    }
}

public class GameEventListener<T0, T1> : MonoBehaviour
{
    [Tooltip("Event to register with.")]
    public GameEvent<T0, T1> Event;

    [Tooltip("Response to invoke when Event is raised")]
    public UnityEvent<T0, T1> Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(T0 item0, T1 item1)
    {
        Response.Invoke(item0, item1);
    }
}
