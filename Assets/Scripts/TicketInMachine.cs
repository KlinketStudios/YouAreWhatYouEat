using UnityEngine;

public class TicketInMachine : MonoBehaviour, IInteractable
{
    [SerializeField] private Event spawnReceiptEvent;
    [SerializeField] private Event receiptTakenEvent;
    [SerializeField] private GameObject receiptPrefab;
    private PlayerData playerData;
    private Animator animator;
    private IClickListener clickListener;
    private bool isReady;

    public IClickListener ClickListener
    {
        get => clickListener;
        set => clickListener = value;
    }
    
    
    public void Interacted(GrabHand grabHand)
    {
        if (playerData.HandedIsHolding(grabHand))
        {
            return;
        }

        if (!isReady)
        {
            return;
        }
        animator.SetTrigger("PickUpTicket");
        GameObject obj = Instantiate(receiptPrefab);

        receiptTakenEvent.dataSlot1 = spawnReceiptEvent.dataSlot1;
        receiptTakenEvent.@event.Invoke();
        obj.GetComponent<InteractableTicket>().Init((int)spawnReceiptEvent.dataSlot1, (string)spawnReceiptEvent.dataSlot2);

        obj.GetComponent<IPickupAndPlaceable>().PickUp(grabHand);
        isReady = false;
    }

    private void Start()
    {
        playerData = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>();
        spawnReceiptEvent.@event += SpawnReceipt;
        animator = transform.parent.GetComponent<Animator>();
    }

    private void SpawnReceipt()
    {
        animator.SetTrigger("SpawnTicket");
        isReady = true;
    }
}
