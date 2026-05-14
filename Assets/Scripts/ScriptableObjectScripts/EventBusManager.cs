using UnityEngine;

public class EventBusManager : MonoBehaviour
{
    public void Awake()
    {
        //find all events and initialize them
        Event[] events = Resources.FindObjectsOfTypeAll<Event>();
        foreach (var @event in events)
        {
            @event.Init();
        }
    }
}