using System;
using UnityEngine;

public class InteractableIngredient : MonoBehaviour, IInteractable, IPickupAndPlaceable
{
    [SerializeField] private Transform origin;
    private GameObject thisObject;
    private int oldLayer;

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
}
