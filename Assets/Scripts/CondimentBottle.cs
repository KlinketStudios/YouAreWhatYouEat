using System;
using UnityEngine;
using UnityEngine.Animations;
using VHierarchy.Libs;

public class CondimentBottle : MonoBehaviour, IInteractable, IUsable, IPickupAndPlaceable
{
    public Transform origin;
    public GameObject thisObject;
    public GameObject condimentPrefab;
    [SerializeField] private float stackDist;
    [SerializeField] private GameObject spriteAndCollider;
    private IClickListener clickListener;
    private bool placeableOnWalls;

    private void Awake()
    {
        SpriteRenderer spriteRenderer = spriteAndCollider.GetComponent<SpriteRenderer>();
        BoxCollider collider = spriteRenderer.GetComponent<BoxCollider>();
        
        collider.size = spriteRenderer.sprite.bounds.size;
        
        thisObject = gameObject;
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

    public Vector3 OldLocalScale { get; set; }

    public bool PlaceableOnWalls
    {
        get => placeableOnWalls;
        set => placeableOnWalls = value;
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
                if (condimentUsedOn.IngredientOn.Plate != null)
                {
                    AddCondiment(condimentUsedOn.IngredientOn.Plate.GetTopItem(false));
                }
                else
                {
                    AddCondiment(condimentUsedOn.IngredientOn.ThisObject);
                }
                return;
                
            }

            if (objUsedOn.TryGetComponent(out IIngredient ingredientUsedOn))
            {
                if (ingredientUsedOn.Plate != null)
                {
                    AddCondiment(ingredientUsedOn.Plate.GetTopItem(false));
                }
                else
                {
                    AddCondiment(objUsedOn);
                }
            }
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
            print("hit");
            GameObject condimentCreated = Instantiate(condimentPrefab,
                objUsedOn.transform.position  +
                new Vector3(0, stackDist * (ingredientUsedOn.CondimentStack.Count + 1), 0),
                Quaternion.identity);

            condimentCreated.transform.parent = objUsedOn.transform;
            try
            {
                condimentCreated.GetComponent<ICondiment>().IngredientOn = ingredientUsedOn;
                condimentCreated.GetComponent<ICondiment>().ThisObject = condimentCreated;
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

            ingredientUsedOn.CondimentStack.Insert(ingredientUsedOn.CondimentStack.Count,
                condimentPrefab.GetComponent<ICondiment>());
            Debug.Log(ingredientUsedOn.CondimentStack[0].ThisObject.name);
        }
    }
}