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
    private IClickListener clickListener;

    private void Awake()
    {
        playerData = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>();
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
            ingredientStack.AddAt<OrderableIngredients>((OrderableIngredients)ingredient.Type, ingredientStack.Count);
            ingredientStackObjs.AddAt<GameObject>(obj, ingredientStackObjs.Count);

            ingredient.ClickListener = this;
            
            if (ingredient.CondimentStack.Count != 0)
            {
                foreach (ICondiment condiment in ingredient.CondimentStack)
                {
                    ingredientStack.AddAt<OrderableIngredients>((OrderableIngredients)condiment.Type, ingredientStack.Count);
                    ingredientStackObjs.AddAt<GameObject>(condiment.ThisObject, ingredientStackObjs.Count);
                }
            }
        }
        //item is condiment and have to add the condiment to the top ingredient and the ingredient stack
        else if (obj.TryGetComponent(out ICondiment condiment))
        {
            List<ICondiment> topItemCondimentStack = 
                GetTopItem(false).GetComponent<IIngredient>().CondimentStack;

            int indexPosition = ingredientStackObjs.Count;
            
            ingredientStack.AddAt<OrderableIngredients>((OrderableIngredients)condiment.Type,
                indexPosition);

            ingredientStackObjs.AddAt<GameObject>(obj, indexPosition);
        }

    }

    public void RemoveIngredient(int ingredientIndexToRemove, GrabHand grabHand)
    {
        GameObject ingredientRemoved = ingredientStackObjs[ingredientIndexToRemove];
        IIngredient ingredientRemovedIngredient = ingredientRemoved.GetComponent<IIngredient>();
     

        ingredientRemoved.transform.parent = null;
        ingredientRemovedIngredient.ClickListener = null;
        ingredientRemovedIngredient.Plate = null;
        ingredientRemoved.GetComponent<IPickupAndPlaceable>().PickUp(grabHand);
        /*if (ingredientRemovedIngredient.CondimentStack.Count > 0)
        {
            foreach (ICondiment condiment in ingredientRemovedIngredient.CondimentStack)
            {
                int index = ingredientStackObjs.IndexOf(condiment.ThisObject);
                print(index);                
                ingredientStack.RemoveAt(index);
                ingredientStackObjs.RemoveAt(index);
            }
        }*/
        for (int i = 0; i < ingredientRemovedIngredient.CondimentStack.Count + 1; i++)
        {
            ingredientStack.RemoveAt(ingredientIndexToRemove);
            ingredientStackObjs.RemoveAt(ingredientIndexToRemove);
        }
        
    }

    private void RecalculateAlternativeness()
    {
        print("recalculate alternativeness");
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
    public int OldLayer
    {
        get => oldLayer;
        set => oldLayer = value;
    }
}