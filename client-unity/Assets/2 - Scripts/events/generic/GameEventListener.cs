using UnityEngine;
using UnityEngine.Events;

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
