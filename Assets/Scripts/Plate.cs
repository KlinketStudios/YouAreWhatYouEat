using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;
using VHierarchy.Libs;

public class Plate : MonoBehaviour, IInteractable, IClickListener, IPickupAndPlaceable, ITrashable
{
    [SerializeField] private float ingredientOffset = .005f;
    public List<OrderableIngredients> ingredientStack = new();
    [HideInInspector] public List<GameObject> ingredientStackObjs = new();
    [SerializeField] private Transform stackPosition;

    private PlayerData playerData;
    public Transform origin;
    public GameObject thisObject;
    private int oldLayer;
    private IClickListener clickListener;


    [Header("Alternativeness Calculation")]
    [SerializeField] private Slider alternativenessSlider;
    public float alternativeness;
    private bool placeableOnWalls;

    private void Awake()
    {
        playerData = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>();
        if (ingredientStack.Count == 0)
        {
            alternativenessSlider.gameObject.SetActive(false);
        }
    }

    public void Click(GrabHand grabHand)
    {
        RemoveIngredient(ingredientStackObjs.IndexOf(GetTopItem(false)), grabHand);
    }

    public void ClickWithObjectInHand(GameObject obj, GrabHand grabHand)
    {
        InteractedWithObjectInHand(obj, grabHand);
        if (obj.TryGetComponent(out IAlternativeIngredient alternativeIngredient))
        {
            RecalculateAlternativeness();
        }
    }

    public IClickListener ClickListener
    {
        get => clickListener;
        set => clickListener = value;
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
            ingredient.PutDown(stackPosition.position + new Vector3(0,(ingredientOffset * ingredientStack.Count), 0),Vector3.up, grabHand);
            AddItem(obj);
            ingredient.Plate = this;
            ingredient.ClickListener = this;
            obj.transform.parent = gameObject.transform;
        }
    }

    [ContextMenu("test")]
    public GameObject GetTopItem(bool countCondiments)
    {
        for (var i = ingredientStack.Count - 1; i >= 0; i--)
        {
            var ingredient = ingredientStack[i];

            if (ingredient == OrderableIngredients.Ketchup || 
                ingredient == OrderableIngredients.Mayo || 
                ingredient == OrderableIngredients.Mustard)
            {
                //ingredient is condiment
                if (countCondiments)
                {
                    return ingredientStackObjs[i];
                }
            }
            else
            {
                return ingredientStackObjs[i];
            }

        }

        return null;
    }

    public void AddItem(GameObject obj)
    {
        if (obj.TryGetComponent(out IIngredient ingredient))
        {
            ingredientStack.Insert(ingredientStack.Count, (OrderableIngredients)ingredient.Type);
            ingredientStackObjs.Insert(ingredientStackObjs.Count, obj);

            ingredient.ClickListener = this;
            
            if (ingredient.CondimentStack.Count != 0)
            {
                foreach (ICondiment condiment in ingredient.CondimentStack)
                {
                    ingredientStack.Insert(ingredientStack.Count, (OrderableIngredients)condiment.Type);
                    ingredientStackObjs.Insert(ingredientStackObjs.Count, condiment.ThisObject);
                }
            }
            alternativenessSlider.gameObject.SetActive(true);
        }
        //item is condiment and have to add the condiment to the top ingredient and the ingredient stack
        else if (obj.TryGetComponent(out ICondiment condiment))
        {
            List<ICondiment> topItemCondimentStack = 
                GetTopItem(false).GetComponent<IIngredient>().CondimentStack;

            int indexPosition = ingredientStackObjs.Count;
            
            ingredientStack.Insert(indexPosition, (OrderableIngredients)condiment.Type);
            ingredientStackObjs.Insert(indexPosition, obj);
        }

        RecalculateAlternativeness();
    }

    public void RemoveIngredient(int ingredientIndexToRemove, GrabHand grabHand)
    {
        GameObject ingredientRemoved = ingredientStackObjs[ingredientIndexToRemove];
        IIngredient ingredientRemovedIngredient = ingredientRemoved.GetComponent<IIngredient>();
     

        ingredientRemoved.transform.parent = null;
        ingredientRemovedIngredient.ClickListener = null;
        ingredientRemovedIngredient.Plate = null;
        ingredientRemoved.GetComponent<IPickupAndPlaceable>().PickUp(grabHand);

        for (int i = 0; i < ingredientRemovedIngredient.CondimentStack.Count + 1; i++)
        {
            ingredientStack.RemoveAt(ingredientIndexToRemove);
            ingredientStackObjs.RemoveAt(ingredientIndexToRemove);
        }

        if (ingredientStack.Count == 0)
        {
            alternativenessSlider.gameObject.SetActive(false);
        }
        RecalculateAlternativeness();
    }

    private void RecalculateAlternativeness()
    {
        float alternativeSum = 0;
        float normalSum = 0;
        
        float totalCount = 0;
        
        IIngredient[] ingredients = new IIngredient[ingredientStackObjs.Count];

        for (int i = 0; i < ingredientStackObjs.Count; i++)
        {
            ingredients[i] = ingredientStackObjs[i].GetComponent<IIngredient>();
        }
        
        foreach (IIngredient ingredient in ingredients)
        {
            if (ingredient.ThisObject.TryGetComponent(out InteractableAlternativeIngredient alternativeIngredient))
            {
                alternativeSum += alternativeIngredient.Alternativeness;
                totalCount += alternativeIngredient.Alternativeness;
            }
            else if (ingredient.ThisObject.TryGetComponent(out ICondiment thisCondiment))
            {
                //Skip condiments
            }
            else
            {
                normalSum++;
                totalCount++;
            }
        }

        if (!float.IsNaN(alternativeSum / totalCount))
        {
            alternativenessSlider.value = alternativeSum / totalCount;
            alternativeness = alternativeSum / totalCount;
        }
        else
        {
            alternativenessSlider.value = 0;
            alternativeness = 0;
        }
    }

    public bool CheckIfValid()
    {
        
        if((IngredientTypes)ingredientStack[0] == IngredientTypes.BreadSlice)
            if ((IngredientTypes)ingredientStack[ingredientStack.Count - 1] == IngredientTypes.BreadSlice)
                return true;

        
        foreach (var ingredient in ingredientStack)
        {
            print(ingredient.HumanName());
        }
        
        return false;
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
    public Vector3 OldLocalScale { get; set; }

    public bool PlaceableOnWalls
    {
        get => placeableOnWalls;
        set => placeableOnWalls = value;
    }

    public int OldLayer
    {
        get => oldLayer;
        set => oldLayer = value;
    }
}