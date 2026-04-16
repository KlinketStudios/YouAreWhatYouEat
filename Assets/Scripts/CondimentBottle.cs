using System;
using UnityEngine;
using VHierarchy.Libs;

public class CondimentBottle : MonoBehaviour, IInteractable, IUsable, IPickupAndPlaceable
{
    public Transform origin;
    public GameObject thisObject;
    public GameObject condimentPrefab;


    public void Interacted(GrabHand grabHand)
    {
        GetComponent<IPickupAndPlaceable>().PickUp(grabHand);
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

    public int OldLayer { get; set; }

    public void Grabbed()
    {
    }

    public void Placed()
    {
    }

    public void Use(GrabHand grabHand, bool usedOnObject, bool usedOnInteractable, GameObject objectUsedOn)
    {
        print("use");
        if (!usedOnObject)
            //shot in air
            //maybe make it shoot a projectile that puts a splat where it lands
            return;
        if (usedOnInteractable)
        {
            if (objectUsedOn.TryGetComponent(out IIngredient ingredientUsedOn))
            {
                Instantiate(condimentPrefab, objectUsedOn.transform.position - origin.transform.localPosition,
                    Quaternion.identity);
                if (ingredientUsedOn.Plate != null)
                {
                    Plate plate = ingredientUsedOn.Plate;
                    
                    
                }
                try
                {
                    ingredientUsedOn.CondimentStack.AddAt<ICondiment>(condimentPrefab.GetComponent<ICondiment>(),
                        ingredientUsedOn.CondimentStack.Count);
                }
                catch (Exception)
                {
                    print("selected condiment prefab is not of type ICondiment");
                }

                return;
            }

            var interactableUsedOn = objectUsedOn.GetComponent<IInteractable>();
        }
    }
}