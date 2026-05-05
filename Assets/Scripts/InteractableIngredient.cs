using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class InteractableIngredient : MonoBehaviour, IIngredient
{
    [SerializeField] private Transform origin;
    [SerializeField] private SpriteRenderer spriteRenderer;
    public IngredientTypes type;
    private Plate currentPlate;
    private readonly bool isPickupable = true;
    private List<ICondiment> condimentStack = new List<ICondiment>();
    public IIngredient listener;
    private IClickListener clickListener;
    public GameObject thisObject;
    private int oldLayer;

    [SerializeField] private Sprite[] sprites;
    private bool placeableOnWalls;


    private void Awake()
    {
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
        BoxCollider collider = spriteRenderer.GetComponent<BoxCollider>();
        collider.size = spriteRenderer.sprite.bounds.size;
        
        ThisObject = gameObject;
    }

    public void Interacted(GrabHand grabHand)
    {
        if (currentPlate == null && isPickupable)
            GetComponent<IPickupAndPlaceable>().PickUp(grabHand);
        else
            clickListener.Click(grabHand);
    }

    public void InteractedWithObjectInHand(GameObject obj, GrabHand grabHand)
    {
        if (currentPlate == null && isPickupable)
            print("Interacted with object in hand");
        else
            clickListener.ClickWithObjectInHand(obj, grabHand);
    }

    public void Click(GrabHand grabHand)
    {
        Interacted(grabHand);
    }

    public void ClickWithObjectInHand(GameObject obj, GrabHand grabHand)
    {
        InteractedWithObjectInHand(obj, grabHand);
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


    public IngredientTypes Type
    {
        get => type;
        set => type = value;
    }

    public List<ICondiment> CondimentStack
    {
        get => condimentStack;
        set => condimentStack = value;
    }
    public Vector3 OldLocalScale { get; set; }

    public bool PlaceableOnWalls
    {
        get => placeableOnWalls;
        set => placeableOnWalls = value;
    }

    public Plate Plate
    {
        get => currentPlate;
        set => currentPlate = value;
    }

    public IClickListener ClickListener
    {
        get => clickListener;
        set => clickListener = value;
    }
}