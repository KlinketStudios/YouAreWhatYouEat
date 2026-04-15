using System;
using UnityEngine;

public class InteractableIngredient : MonoBehaviour, IIngredient
{
    [SerializeField] private Transform origin;
    public IngredientTypes type;
    private GameObject thisObject;
    private int oldLayer;
    private IInteractable listener;

    private void Awake()
    {
        thisObject = gameObject;
    }

    public void Interacted(GrabHand grabHand)
    {
        GetComponent<IPickupAndPlaceable>().PickUp(grabHand);
    }

    public void InteractedWithObjectInHand(GameObject obj, GrabHand grabHand)
    {
        print("Interacted with object in hand");
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
