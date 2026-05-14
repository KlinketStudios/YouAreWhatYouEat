using UnityEngine;

public class CuttableIngredient : MonoBehaviour, ICuttable
{
    [SerializeField] private int cutAmount;
    [SerializeField] private GameObject product;
    [SerializeField] private CuttableIngredients cuttableType;
    [SerializeField] private Transform origin;
    [SerializeField] private Sprite sprite;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private GameObject thisObject;
    private int oldLayer;
    private Vector3 oldLocalScale;
    private IClickListener clickListener;
    private int currentCut;
    private bool placeableOnWalls;

    private void Start()
    {
        //set the sprite and fit the box collider to it
        spriteRenderer.sprite = sprite;
        BoxCollider collider = spriteRenderer.GetComponent<BoxCollider>();
        collider.size = spriteRenderer.sprite.bounds.size;
        
        //cache this object
        thisObject = gameObject;
    }
    
    public void Interacted(GrabHand grabHand)
    {
        if (clickListener != null)
        {
            //more than likely to be the cutting board, i think thats the only use for this right now
            //tell any listener that this object was clicked
            clickListener.Click(grabHand);
        }
        else
        {
            //pick up this object
            GetComponent<IPickupAndPlaceable>().PickUp(grabHand);
        }
    }

    public void InteractedWithObjectInHand(GameObject obj, GrabHand grabHand)
    {
        if (clickListener != null)
        {
            //tell anything listening that it was clicked with an object in hand
            //so the cutting board can figure out if the knife was in hand 
            clickListener.ClickWithObjectInHand(obj, grabHand);
        }
    }

    public int CutAmount
    {
        get => cutAmount;
        set => cutAmount = value;
    }

    public CuttableIngredients CuttableType
    {
        get => cuttableType;
        set => cuttableType = value;
    }

    public GameObject Product
    {
        get => product;
        set => product = value;
    }

    public int CurrentCut
    {
        get => currentCut;
        set => currentCut = value;
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

    public Vector3 OldLocalScale
    {
        get => oldLocalScale;
        set => oldLocalScale = value;
    }

    public bool PlaceableOnWalls
    {
        get => placeableOnWalls;
        set => placeableOnWalls = value;
    }

    public IClickListener ClickListener
    {
        get => clickListener;
        set => clickListener = value;
    }

    
}