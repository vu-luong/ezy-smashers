using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    [Tooltip("Event to register with.")]
    [SerializeField]
    private GameEvent gameEvent;

    [Tooltip("Response to invoke when Event is raised")]
    [SerializeField]
    private UnityEvent response;

    private void OnEnable()
    {
        gameEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        gameEvent.UnregisterListener(this);
    }

    public void OnEventRaised()
    {
        response.Invoke();
    }
}

public class GameEventListener<T> : MonoBehaviour
{
    [Tooltip("Event to register with.")]
    [SerializeField]
    private GameEvent<T> gameEvent;

    [Tooltip("Response to invoke when Event is raised")]
    [SerializeField]
    private UnityEvent<T> response;

    private void OnEnable()
    {
        gameEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        gameEvent.UnregisterListener(this);
    }

    public void OnEventRaised(T item)
    {
        response.Invoke(item);
    }
}

public class GameEventListener<T0, T1> : MonoBehaviour
{
    [Tooltip("Event to register with.")]
    [SerializeField]
    private GameEvent<T0, T1> gameEvent;

    [Tooltip("Response to invoke when Event is raised")]
    [SerializeField]
    private UnityEvent<T0, T1> response;

    private void OnEnable()
    {
        gameEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        gameEvent.UnregisterListener(this);
    }

    public void OnEventRaised(T0 item0, T1 item1)
    {
        response.Invoke(item0, item1);
    }
}
