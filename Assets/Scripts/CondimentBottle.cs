using System;
using UnityEngine;

public class CondimentBottle : MonoBehaviour, IInteractable, IUsable, IPickupAndPlaceable
{
    public Transform origin;
    public GameObject thisObject;
    public GameObject condimentPrefab;


    public void Interacted(GrabHand grabHand)
    {
        throw new NotImplementedException();
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
        throw new NotImplementedException();
    }

    public void Placed()
    {
        throw new NotImplementedException();
    }

    public void Use(GrabHand grabHand, bool usedOnObject, bool usedOnInteractable, GameObject objectUsedOn)
    {
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
                    ingredientUsedOn.CondimentStack.Add(condimentPrefab.GetComponent<ICondiment>());
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