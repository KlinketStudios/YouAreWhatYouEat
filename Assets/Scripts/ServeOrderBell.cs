using System;
using UnityEngine;

public class ServeOrderBell : MonoBehaviour, IInteractable
{
    private IClickListener clickListener;
    [SerializeField] private ServeOrderBoard parent;


    public void Awake()
    {
        //cache serve board script
        parent = transform.GetComponentInParent<ServeOrderBoard>();
    }

    public void Interacted(GrabHand grabHand)
    {
        //tell the serve board the bell was interacted with
        parent.BellInteractedWith(grabHand);
    }
    
    public IClickListener ClickListener
    {
        get => clickListener;
        set => clickListener = value;
    }
}