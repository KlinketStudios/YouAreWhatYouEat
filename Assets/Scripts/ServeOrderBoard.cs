using System;
using KlinketStudiosTools;
using UnityEngine;

public class ServeOrderBoard : MonoBehaviour
{
    private PlayerData player;
    
    private IInteractable ticketSpike;
    private IInteractable bell;

    [SerializeField] private GameObject ticketSpikeObject;
    [SerializeField] private GameObject bellObject;
    [SerializeField] private BoxCollider foodCheckZone;

    private int currentOrder = -1;

    [SerializeField] private Event serveOrderEvent;

    [SerializeField] private LayerMask foodCheckZoneMask;


    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>();
        ticketSpike = ticketSpikeObject.GetComponent<IInteractable>();
        bell = bellObject.GetComponent<IInteractable>();
    }

    public void TicketSpikeInteractedWith(GrabHand grabHand)
    {
        GameObject heldItem = player.HandedHeldObject(grabHand);
        if (heldItem.TryGetComponent(out InteractableTicket heldTicket))
        {
            currentOrder = heldTicket.orderID;
            heldTicket.GetComponent<IPickupAndPlaceable>().PutDown(Vector3.zero, Vector2.up, grabHand);
            Destroy(heldItem);
        }
        else
        {
            //show text to player saying that held item is not receipt
            Debug.LogWarning("must interact with receipt in hand");
        }
    }

    public void BellInteractedWith(GrabHand grabHand)
    {
        if (currentOrder != -1)
        {
            if (CheckIfThereIsFoodOnBoard(out GameObject foodObject))
            {
                var plate = foodObject.GetComponent<Plate>();
                if (plate.CheckIfValid())
                {
                    serveOrderEvent.dataSlot1 = currentOrder;
                    serveOrderEvent.dataSlot2 = foodObject;
                    serveOrderEvent.dataSlot3 = plate.ingredientStack;
                    serveOrderEvent.@event.Invoke();
                        
                    currentOrder = -1;
                    //print("food is valid and serve event called");
                }

                //print("food failed validation");
            }
        }
        else
        {
            //show text to player saying that they have to spike the receipt before sending out order
            Debug.LogWarning("must spike receipt before sending out order");
        }
    }

    private bool CheckIfThereIsFoodOnBoard(out GameObject foodObject)
    {

        Collider[] collidersInFoodCheckZone = Physics.OverlapBox(
            foodCheckZone.center + foodCheckZone.transform.position,
            foodCheckZone.bounds.extents, transform.rotation, foodCheckZoneMask);

        int foodsInZone = 0;
        GameObject foodObjectOnBoard = null;
        foreach (var foodCollider in collidersInFoodCheckZone)
        {
            if (foodCollider.CompareTag("Order"))
            {
                foodsInZone++;
                foodObjectOnBoard = foodCollider.gameObject;
            }
        }
        if (foodsInZone == 1)
        {
            if(foodObjectOnBoard == null)
                Debug.LogError("Plate of food on board is not tagged Order");
            else
            {
                foodObject = foodObjectOnBoard;
                return true;
            }
        }
        else if (foodsInZone > 1)
        {
            //show text to player saying that they have remove some of the food on the board so that there is only one
            Debug.LogWarning("Too many orders on the board, please make sure there is only one order when you ring it up");
            foodObject = null;
            return false;
        }
        //show text to player saying that there is no food on the board and that they have to put the order on the board
        Debug.LogWarning("There is no order on the board, please place your order on the board");
        foodObject = null;
        return false;
    }
    
}