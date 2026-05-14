using System;
using UnityEngine;

public class PlateStack : MonoBehaviour, IInteractable
{
    private IClickListener clickListener;
    private PlayerData playerData;
    [SerializeField] private GameObject platePrefab;
 
    
    private void Awake()
    {
        //find playerdata while avoiding FindGameObjectOfType because its very expensive 
        playerData = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>();
    }
    
    public void Interacted(GrabHand grabHand)
    {
        //check if the player is holding anything in grabHand
        if (playerData.HandedIsHolding(grabHand))
        {
            return;
        }

        //create new plate and cache its reference
        GameObject plate = Instantiate(platePrefab);

        //get the plates iPickupableAndPlaceable 
        IPickupAndPlaceable plateIPickupable = plate.GetComponent<IPickupAndPlaceable>();

        //pickup the plate
        plateIPickupable.PickUp(grabHand);
    }
    
    public IClickListener ClickListener
    {
        get => clickListener;
        set => clickListener = value;
    }

}
