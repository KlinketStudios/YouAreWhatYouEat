using System;
using UnityEngine;

public class PlateStack : MonoBehaviour, IInteractable
{
    private IClickListener clickListener;
    private PlayerData playerData;
    [SerializeField] private GameObject platePrefab;
    
    
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

        GameObject plate = Instantiate(platePrefab);

        IPickupAndPlaceable plateIPickupable = plate.GetComponent<IPickupAndPlaceable>();

        plateIPickupable.PickUp(grabHand);
    }

    private void Awake()
    {
        playerData = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>();
    }
}
