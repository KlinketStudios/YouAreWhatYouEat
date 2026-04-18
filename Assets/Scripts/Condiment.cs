using System;
using UnityEngine;

public class Condiment : MonoBehaviour, ICondiment
{

    public IngredientTypes type;
    private IClickListener clickListener;
    public IIngredient ingredientOn;
    private GameObject thisObject;

    private void Awake()
    {
        thisObject = gameObject;
    }

    public void Interacted(GrabHand grabHand)
    {
        ClickListener?.Click(grabHand);
    }

    public void InteractedWithObjectInHand(GameObject obj, GrabHand grabHand)
    {
        ClickListener?.ClickWithObjectInHand(obj, grabHand);
    }

    public IngredientTypes Type
    {
        get => type;
        set => type = value;
    }

    public IClickListener ClickListener
    {
        get => clickListener;
        set => clickListener = value;
    }

    public IIngredient IngredientOn
    {
        get => ingredientOn;
        set => ingredientOn = value;
    }

    public GameObject ThisObject
    {
        get => thisObject;
        set => thisObject = value;
    }
}