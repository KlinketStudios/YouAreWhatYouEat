using System;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Event", menuName = "ScriptableObjects/Event")]
public class Event : ScriptableObject
{
    public Action @event;

    [HideInInspector] public bool wasTriggeredThisFrame;
    
    public object dataSlot1;
    public object dataSlot2;
    public object dataSlot3;
    public object dataSlot4;
    public object dataSlot5;

    public void Init()
    {
        Debug.Log("initialized");
        @event += EventTriggered;
    }

    public async void EventTriggered()
    {
        wasTriggeredThisFrame = true;
        
        await Task.Yield();
        await Task.Yield();
        
        wasTriggeredThisFrame = false;
        }
}