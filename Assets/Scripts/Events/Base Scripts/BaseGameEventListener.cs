using UnityEngine;
using UnityEngine.Events;

// T = Generic type, E = Event, UER = Unity Event Response
public abstract class BaseGameEventListener<T, E, UER> : MonoBehaviour,
    IGameEventListener<T> where E : BaseGameEvent<T> where UER : UnityEvent<T>
{
    [SerializeField] private E gameEvent;
    [SerializeField] private UER unityEventResponse;

    public E GameEvent { get { return gameEvent; } set { gameEvent = value; } }

    private void OnEnable()
    {
        GameEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        GameEvent.UnregisterListener(this);
    }

    public void OnEventRaised(T item)
    {
        if (unityEventResponse != null)
            unityEventResponse.Invoke(item);
    }
}
