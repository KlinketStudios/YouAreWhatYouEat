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
    
    private void Start()
    {
        //find playerdata while avoiding FindGameObjectOfType because its very expensive 
        playerData = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>();
        //subscribe to spawn receipt event
        spawnReceiptEvent.@event += SpawnReceipt;
        //find the receipt machines animator 
        animator = transform.parent.GetComponent<Animator>();
    }
       
    public void Interacted(GrabHand grabHand)
    {
        
        //dont do anything if the player interacted wtih this with something in their hand
        if (playerData.HandedIsHolding(grabHand))
        {
            return;
        }

        //checks if the printer can print
        if (!isReady)
        {
            return;
        }
        
        //resets the print animation
        animator.SetTrigger("PickUpTicket");
        //creates object and caches it
        GameObject obj = Instantiate(receiptPrefab);

        //send event that player has taken the ticket, also send along the order id
        receiptTakenEvent.dataSlot1 = spawnReceiptEvent.dataSlot1;
        receiptTakenEvent.@event.Invoke();
        
        //initializes the ticket interactable object
        obj.GetComponent<InteractableTicket>().Init((int)spawnReceiptEvent.dataSlot1, (string)spawnReceiptEvent.dataSlot2);

        //picks up the ticket
        obj.GetComponent<IPickupAndPlaceable>().PickUp(grabHand);
        
        //reset fully
        isReady = false;
    }

    private void SpawnReceipt()
    {
        //play the receipt print animation && prepare for spawning of the ticket
        animator.SetTrigger("SpawnTicket");
        isReady = true;
    }
}
