using System;
using UnityEngine;

public class ServeOrderBell : MonoBehaviour, IInteractable
{
    private IClickListener clickListener;
    [SerializeField] private ServeOrderBoard parent;

    public IClickListener ClickListener
    {
        get => clickListener;
        set => clickListener = value;
    }

    public void Interacted(GrabHand grabHand)
    {
        parent.BellInteractedWith(grabHand);
    }

    public void Awake()
    {
        parent = transform.GetComponentInParent<ServeOrderBoard>();
    }
}