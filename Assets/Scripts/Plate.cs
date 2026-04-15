using System;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour, IInteractable, IClickListener
{

    [SerializeField] private float ingredientOffset = .005f;
    [HideInInspector] public List<OrderableIngredients> ingredientStack = new List<OrderableIngredients>();

    private PlayerData playerData;

    private void Awake()
    {
        playerData = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>();
    }

    public void Interacted(GrabHand grabHand)
    {
        GameObject heldItem = playerData.HandedHeldObject(grabHand);
        if (heldItem.TryGetComponent(out IIngredient ingredient))
        {
            //grab top item
        }
    }

    public void InteractedWithObjectInHand(GameObject obj, GrabHand grabHand)
    {
        //check if is IIngredient
        if (obj.TryGetComponent(out IIngredient ingredient))
        {
            //add Item to stack
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

    public IngredientTypes GetTopItem()
    {
        for (int i = ingredientStack.Count - 1; i >= 0; i++)
        {
            OrderableIngredients ingredient = ingredientStack[i];

            if ((int)ingredient == 14 || (int)ingredient == 15 || (int)ingredient == 16)
            {
                //ingredient is condiment
            }
            else
            {
                //ingredient is not condiment
                
            }
        }
    
        //temp
        return IngredientTypes.BreadLoaf;
    }
}
