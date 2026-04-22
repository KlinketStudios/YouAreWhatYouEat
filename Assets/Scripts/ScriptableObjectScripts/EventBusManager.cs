using UnityEngine;

public class EventBusManager : MonoBehaviour
{
    public void Awake()
    {
        Event[] events = Resources.FindObjectsOfTypeAll<Event>();
        foreach (var @event in events)
        {
            @event.Init();
        }
    }
}