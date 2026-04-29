using UnityEngine;

public class ServeOrderTicketSpike : MonoBehaviour, IInteractable
{
    [SerializeField] private IClickListener clickListener;
    [SerializeField] private ServeOrderBoard parent;

    public IClickListener ClickListener
    {
        get => clickListener;
        set => clickListener = value;
    }

    public void Interacted(GrabHand grabHand)
    {
        parent.TicketSpikeInteractedWith(grabHand);
    }


    public void Awake()
    {
        parent = transform.GetComponentInParent<ServeOrderBoard>();
    }
}