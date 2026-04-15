using System;
using System.Collections.Generic;
using UnityEngine;

public class InteractableAlternativeIngredient : MonoBehaviour, IAlternativeIngredient
{
    private Plate plate;

    public void Interacted(GrabHand grabHand)
    {
        throw new NotImplementedException();
    }

    public Transform Origin { get; set; }

    public GameObject ThisObject { get; set; }

    public int OldLayer { get; set; }

    public void Grabbed()
    {
        throw new NotImplementedException();
    }

    public void Placed()
    {
        throw new NotImplementedException();
    }

    public void Click(GrabHand grabHand)
    {
        throw new NotImplementedException();
    }

    public void ClickWithObjectInHand(GameObject obj, GrabHand grabHand)
    {
        throw new NotImplementedException();
    }

    public IngredientTypes Type { get; set; }

    public List<ICondiment> CondimentStack { get; set; }

    public Plate Plate
    {
        get => plate;
        set => plate = value;
    }
}