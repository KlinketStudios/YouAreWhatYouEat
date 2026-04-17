using System;
using Unity.VisualScripting;
using UnityEngine;
using VHierarchy.Libs;

public class CondimentBottle : MonoBehaviour, IInteractable, IUsable, IPickupAndPlaceable
{
    public Transform origin;
    public GameObject thisObject;
    public GameObject condimentPrefab;
    [SerializeField] private float stackDist;


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


    public void Use(GrabHand grabHand)
    {
        print("used in the air");
    }

    public void UsedOnObject(GrabHand grabHand, GameObject objUsedOn)
    {

        bool usedOnInteractable = objUsedOn.TryGetComponent(out IInteractable interactableUsedOn);

        if (usedOnInteractable)
        {
            if (objUsedOn.TryGetComponent(out ICondiment condimentUsedOn))
            {
                if (condimentUsedOn.ClickListener != null)
                {
                    AddCondiment(condimentUsedOn.IngredientOn.ThisObject);
                    return;
                }
            }
            AddCondiment(objUsedOn);
        }
        else
        {
            GetComponent<IPickupAndPlaceable>().PutDownAtLookPoint(grabHand);
        }
    }
    

    public void AddCondiment(GameObject objUsedOn)
    {
        if (objUsedOn.TryGetComponent(out IIngredient ingredientUsedOn))
        {
            GameObject condimentCreated = Instantiate(condimentPrefab,
                (objUsedOn.transform.position - origin.transform.localPosition) +
                new Vector3(0, stackDist * ingredientUsedOn.CondimentStack.Count, 0),
                Quaternion.identity);

            condimentCreated.transform.parent = objUsedOn.transform;
            try
            {
                condimentCreated.GetComponent<ICondiment>().IngredientOn = ingredientUsedOn;
                condimentCreated.GetComponent<ICondiment>().ClickListener = objUsedOn.GetComponent<IClickListener>();
            }
            catch (Exception)
            {
                Debug.LogError("selected condiment prefab is not of type ICondiment");
                return;
            }

            if (ingredientUsedOn.Plate != null)
            {
                Plate plate = ingredientUsedOn.Plate;

                plate.AddItem(condimentCreated);
            }

            ingredientUsedOn.CondimentStack.AddAt<ICondiment>(condimentPrefab.GetComponent<ICondiment>(),
                ingredientUsedOn.CondimentStack.Count);
        }
    }
}