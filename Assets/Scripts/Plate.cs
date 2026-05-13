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
        //find playerdata while avoiding FindGameObjectOfType because its very expensive
        playerData = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>();
        
        //turn off the alternativeness slider if there are no ingredients on the plate
        if (ingredientStack.Count == 0)
        {
            alternativenessSlider.gameObject.SetActive(false);
        }
    }

    public void Click(GrabHand grabHand)
    {
        //clicked an ingredient on the plate without holding anything
        
        //remove the top ingredient
        RemoveIngredient(ingredientStackObjs.IndexOf(GetTopItem(false)), grabHand);
    }

    public void ClickWithObjectInHand(GameObject obj, GrabHand grabHand)
    {
        //clicked an ingredient on the plate while holding something
        
        //call the normal interacted function
        InteractedWithObjectInHand(obj, grabHand);
        
        //if the object is alternative recalculate the total alternativeness of the plate 
        //this is already happening on line 173
        if (obj.TryGetComponent(out IAlternativeIngredient alternativeIngredient))
        {
            RecalculateAlternativeness();
        }
    }


    public void Interacted(GrabHand grabHand)
    {
        //clicked on the plate model, pick up the plate
        GetComponent<IPickupAndPlaceable>().PickUp(grabHand);
    }

    public void InteractedWithObjectInHand(GameObject obj, GrabHand grabHand)
    {
        //check if the object that the player is trying to place on the plate is an ingredient
        if (obj.TryGetComponent(out IIngredient ingredient))
        {
            //remove the object from the players hand
            ingredient.PutDown(stackPosition.position + new Vector3(0,(ingredientOffset * ingredientStack.Count), 0),Vector3.up, grabHand);
            
            //add the item to the plate
            AddItem(obj);
            
            //set the objects plate and clickListener variables to be this plate
            ingredient.Plate = this;
            ingredient.ClickListener = this;
            
            //set the parent so that it follows its movements
            obj.transform.parent = gameObject.transform;
        }
    }

    public GameObject GetTopItem(bool countCondiments)
    {
        //do a reverse for loop search on the ingredient stack
        for (var i = ingredientStack.Count - 1; i >= 0; i--)
        {
            
            //cache the current iterated ingredient
            var ingredient = ingredientStack[i];

            //check if it is a condiment
            if (ingredient == OrderableIngredients.Ketchup || 
                ingredient == OrderableIngredients.Mayo || 
                ingredient == OrderableIngredients.Mustard)
            {
                //ingredient is condiment
                if (countCondiments)
                {
                    //return this object only if the function is looking for condiments
                    return ingredientStackObjs[i];
                }
            }
            else
            {
                //this ingredient is not a condiment
                //return this ingredient
                return ingredientStackObjs[i];
            }

        }

        //there are no ingredients || there are no ingredients that are condiments while not looking for them in the ingredient list
        return null;
    }

    public void AddItem(GameObject obj)
    {
        
        //check if the item the function is trying to add is an ingredient
        if (obj.TryGetComponent(out IIngredient ingredient))
        {
            
            //add the ingredient to the top of the lists
            ingredientStack.Insert(ingredientStack.Count, (OrderableIngredients)ingredient.Type);
            ingredientStackObjs.Insert(ingredientStackObjs.Count, obj);

            //set the clickListener of the ingredients which is already being done on line 83
            //dont know why this is being set twice, but maybe it has a purpose
            ingredient.ClickListener = this;
            
            //check if the ingredient has any condiments on it and add those as well
            if (ingredient.CondimentStack.Count != 0)
            {
                //loop through the condiments on the ingredient
                foreach (ICondiment condiment in ingredient.CondimentStack)
                {
                    
                    //add this condiment to the top of the lists
                    ingredientStack.Insert(ingredientStack.Count, (OrderableIngredients)condiment.Type);
                    ingredientStackObjs.Insert(ingredientStackObjs.Count, condiment.ThisObject);
                }
            }
            
            //turn on the alternativeness slider because at this point there is no way for there to be no ingredients on the plate
            alternativenessSlider.gameObject.SetActive(true);
        }
        
        //item is condiment and has to add the condiment to the top ingredient and the ingredient stack
        else if (obj.TryGetComponent(out ICondiment condiment))
        {
            //find top ingredient
            List<ICondiment> topItemCondimentStack = 
                GetTopItem(false).GetComponent<IIngredient>().CondimentStack;

            //get the top index
            //not really needed but i guess it saves one extra call to ingredientStackObjs.Count
            int indexPosition = ingredientStackObjs.Count;
            
            //add the condiment to the top of the lists
            ingredientStack.Insert(indexPosition, (OrderableIngredients)condiment.Type);
            ingredientStackObjs.Insert(indexPosition, obj);
        }

        //recalculate the total alternativeness of the plate
        RecalculateAlternativeness();
    }

    public void RemoveIngredient(int ingredientIndexToRemove, GrabHand grabHand)
    {
        //cache the items to remove
        GameObject ingredientRemoved = ingredientStackObjs[ingredientIndexToRemove];
        IIngredient ingredientRemovedIngredient = ingredientRemoved.GetComponent<IIngredient>();
     
        //reset all the variables related to being on a plate && remove the parenting
        ingredientRemoved.transform.parent = null;
        ingredientRemovedIngredient.ClickListener = null;
        ingredientRemovedIngredient.Plate = null;
        
        //pick up the ingredient
        ingredientRemoved.GetComponent<IPickupAndPlaceable>().PickUp(grabHand);

        //check if the ingredient the function is trying to remove has any condiments on it 
        //if so remove them too 
        for (int i = 0; i < ingredientRemovedIngredient.CondimentStack.Count + 1; i++)
        {
            //remove current iterated condiment
            ingredientStack.RemoveAt(ingredientIndexToRemove);
            ingredientStackObjs.RemoveAt(ingredientIndexToRemove);
        }

        //check if the plate has nothing on it and turn off the alternativeness slider if so
        if (ingredientStack.Count == 0)
        {
            alternativenessSlider.gameObject.SetActive(false);
        }
        
        //recalculate the total alternativeness of the plate
        RecalculateAlternativeness();
    }

    private void RecalculateAlternativeness()
    {
        //create variables for use later
        float alternativeSum = 0;
        float normalSum = 0;
        
        float totalCount = 0;
        
        //create an array the same size as the ingredients list
        IIngredient[] ingredients = new IIngredient[ingredientStackObjs.Count];

        //copy the ingredients list to the new array
        for (int i = 0; i < ingredientStackObjs.Count; i++)
        {
            ingredients[i] = ingredientStackObjs[i].GetComponent<IIngredient>();
        }
        
        //loop through all the ingredients on the plate
        foreach (IIngredient ingredient in ingredients)
        {
            //check if the current iterated ingredient is alternative 
            if (ingredient.ThisObject.TryGetComponent(out InteractableAlternativeIngredient alternativeIngredient))
            {
                //add the alternativeness to both the alternativenessSum and total variables 
                alternativeSum += alternativeIngredient.Alternativeness;
                totalCount += alternativeIngredient.Alternativeness;
            }
            //checks if the ingredient is a condiment
            else if (ingredient.ThisObject.TryGetComponent(out ICondiment thisCondiment))
            {
                //Skip condiments
            }
            //ingredient has to be normal ingredient
            else
            {
                //add 1 to the normalSum and the total
                normalSum++;
                totalCount++;
            }
        }

        //check for division against 0
        if (!float.IsNaN(alternativeSum / totalCount))
        {
            //get the percent of alternativeness and set that to be the sliders value
            alternativenessSlider.value = alternativeSum / totalCount;
            //cache the alternativeness percent for later when the customer needs it
            alternativeness = alternativeSum / totalCount;
        }
        else
        {
            //catch if divided against 0 
            alternativenessSlider.value = 0;
            alternativeness = 0;
        }
    }

    /// <summary>
    /// checks if the sandwich had bread on top and on bottom
    /// </summary>
    /// <returns>true if valid, and false if not</returns>
    public bool CheckIfValid()
    {
        //check if bottom ingredient is bread
        if((IngredientTypes)ingredientStack[0] == IngredientTypes.BreadSlice)
            //check if top ingredient is bread
            if ((IngredientTypes)ingredientStack[ingredientStack.Count - 1] == IngredientTypes.BreadSlice)
                return true;
        
        return false;
    }

    #region Properties

    public IClickListener ClickListener
    {
        get => clickListener;
        set => clickListener = value;
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

    #endregion
}