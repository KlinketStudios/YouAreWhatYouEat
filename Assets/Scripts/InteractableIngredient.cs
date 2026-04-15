using System;
using UnityEngine;

public class InteractableIngredient : MonoBehaviour, IIngredient
{
    [SerializeField] private Transform origin;
    public IngredientTypes type;
    private GameObject thisObject;
    private int oldLayer;
    private IInteractable listener;
    private Plate currentPlate;
    private IClickListener clickListener;
    private bool isPickupable = true;

    private void Awake()
    {
        thisObject = gameObject;
    }

    public void Interacted(GrabHand grabHand)
    {
        if (currentPlate == null && isPickupable)
        {
            GetComponent<IPickupAndPlaceable>().PickUp(grabHand);
        }
        else
        {
            clickListener.Click(grabHand);
        }
    }

    public void InteractedWithObjectInHand(GameObject obj, GrabHand grabHand)
    {
        if (currentPlate == null && isPickupable)
        {
            print("Interacted with object in hand");
        }
        else
        {
            clickListener.ClickWithObjectInHand(obj, grabHand);
        }
    }

    public void Click(GrabHand grabHand)
    {
        Interacted(grabHand);
    }

    public void ClickWithObjectInHand(GameObject obj, GrabHand grabHand)
    {
        InteractedWithObjectInHand(obj, grabHand);
    }

    public Transform Origin
    {
        get => origin;
        set => origin = value;
    }

    public GameObject ThisObject
    {
        get => thisObject;
        set => thisObject = value;
    }

    public int OldLayer
    {
        get => oldLayer;
        set => oldLayer = value;
    }

    public IInteractable Listener
    {
        get => listener;
        set => listener = value;
    }

    public IngredientTypes Type
    {
        get => type;
        set => type = value;
    }
}
