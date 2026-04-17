using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using VHierarchy.Libs;

public class Plate : MonoBehaviour, IInteractable, IClickListener, IPickupAndPlaceable
{
    [SerializeField] private float ingredientOffset = .005f;
    public List<OrderableIngredients> ingredientStack = new();
    [HideInInspector] public List<GameObject> ingredientStackObjs = new();
    [SerializeField] private Transform stackPosition;

    private PlayerData playerData;
    public Transform origin;
    public GameObject thisObject;
    private int oldLayer;

    private void Awake()
    {
        playerData = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>();
    }

    public void Click(GrabHand grabHand)
    {
        RemoveIngredient(ingredientStack.IndexOf((OrderableIngredients)GetTopItem(false).GetComponent<IIngredient>().Type), grabHand);
    }

    public void ClickWithObjectInHand(GameObject obj, GrabHand grabHand)
    {
        InteractedWithObjectInHand(obj, grabHand);
    }

    public void Interacted(GrabHand grabHand)
    {
        GetComponent<IPickupAndPlaceable>().PickUp(grabHand);
    }

    public void InteractedWithObjectInHand(GameObject obj, GrabHand grabHand)
    {
        //check if is IIngredient
        if (obj.TryGetComponent(out IIngredient ingredient))
        {
            AddItem(obj);
            ingredient.PutDown(Vector3.zero,Vector3.zero, grabHand);
            ingredient.Plate = this;
            ingredient.ClickListener = this;
        }
    }

    [ContextMenu("test")]
    public GameObject GetTopItem(bool countCondiments)
    {
        //this shii not working!!! im krilling myself
        for (var i = ingredientStack.Count; i >= 0; i++)
        {
            var ingredient = ingredientStack[i];

            if ((int)ingredient == 14 || (int)ingredient == 15 || (int)ingredient == 16)
            {
                if (countCondiments)
                {
                    print(i);
                    return ingredientStackObjs[i];
                }

                print(i);
                //ingredient is condiment
                return ingredientStackObjs[i];
            }
        }

        print("none");
        return null;
    }

    public void AddItem(GameObject obj)
    {
        if (obj.TryGetComponent(out IIngredient ingredient))
        {
            ingredientStack.AddAt<OrderableIngredients>((OrderableIngredients)ingredient.Type, ingredientStack.Count);
            ingredientStackObjs.AddAt<GameObject>(obj, ingredientStackObjs.Count);

            ingredient.ClickListener = this;
            
            if (ingredient.CondimentStack.Count != 0)
            {
                foreach (ICondiment condiment in ingredient.CondimentStack)
                {
                    ingredientStack.AddAt<OrderableIngredients>((OrderableIngredients)condiment.Type, ingredientStack.Count);
                    ingredientStackObjs.AddAt<GameObject>(obj, ingredientStackObjs.Count);
                }
            }
        }
        //item is condiment and have to add the condiment to the top ingredient and the ingredient stack
        else if (obj.TryGetComponent(out ICondiment condiment))
        {
            List<ICondiment> topItemCondimentStack = 
                GetTopItem(false).GetComponent<IIngredient>().CondimentStack;
            
            int indexPosition = ingredientStackObjs.IndexOf(obj) + topItemCondimentStack.Count + 2;
            
            ingredientStack.AddAt<OrderableIngredients>((OrderableIngredients)condiment.Type,
                indexPosition);

            ingredientStackObjs.AddAt<GameObject>(obj, indexPosition);
        }
}

    public void RemoveIngredient(int ingredient, GrabHand grabHand)
    {
        GameObject ingredientRemoved = ingredientStackObjs[ingredient];
        IIngredient ingredientRemovedIngredient = ingredientRemoved.GetComponent<IIngredient>();
        
        ingredientStack.RemoveAt(ingredient);
        ingredientStackObjs.RemoveAt(ingredient);

        ingredientRemoved.transform.parent = null;
        ingredientRemoved.GetComponent<IIngredient>().ClickListener = null;
        ingredientRemoved.GetComponent<IIngredient>().Plate = null;
        ingredientRemoved.GetComponent<IPickupAndPlaceable>().PickUp(grabHand);
        if (ingredientRemovedIngredient.CondimentStack.Count != 0)
        {
            foreach (ICondiment condiment in ingredientRemovedIngredient.CondimentStack)
            {
                ingredientStack.RemoveAt(ingredient);
                ingredientStackObjs.RemoveAt(ingredient);
            }
        }
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

    public void Grabbed()
    {
    }

    public void Placed()
    {
    }
}