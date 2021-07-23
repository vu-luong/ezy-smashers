using UnityEngine;
using UnityEngine.Events;

public class GameNoArgsEventListener : MonoBehaviour
{
    [Tooltip("Event to register with.")]
    public GameNoArgsEvent Event;

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
