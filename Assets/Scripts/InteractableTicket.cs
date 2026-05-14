using UnityEngine;
using UnityEngine.Animations;
using TMPro;

public class InteractableTicket : MonoBehaviour, IInteractable, IPickupAndPlaceable
{
    private IClickListener clickListener;
    [SerializeField] private Transform origin;
    private GameObject thisObject;
    private int oldLayer;
    private Vector3 oldLocalScale;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private TMP_Text orderText;
    [SerializeField] private TMP_Text orderIDText;
    public int orderID; 
    public string order;
    private bool placeableOnWalls = true;

    void Awake()
    {
        //cache this object
        thisObject = gameObject;
        
        //set the box collider to be the size of the sprite
        BoxCollider collider = spriteRenderer.GetComponent<BoxCollider>();
        collider.size = spriteRenderer.sprite.bounds.size;
    }
    
    public void Interacted(GrabHand grabHand)
    {
        //pick up on interaction
        GetComponent<IPickupAndPlaceable>().PickUp(grabHand);
    }

    /// <summary>
    /// initialize ticket
    /// </summary>
    /// <param name="orderID">the order ID</param>
    /// <param name="order">the order text</param>
    public void Init(int orderID, string order)
    {
        //cache this object
        thisObject = gameObject;
        
        //set the visual texts
        orderIDText.text = orderID.ToString();
        orderText.text = order;
        
        //cache the values
        this.orderID = orderID;
        this.order = order;
    }
    
    //make the ticket look at the player if it was placed on the ground && look at the normal of the wall if placed on a wall
    public void Placed(Vector3 normal)
    {
        //check if placed on a wall 
        if (Mathf.Abs(Vector3.Dot(normal, Vector3.up)) >= .25f)
            //object should already be facing the correct way, return
            return;
        
        //cache the AimController
        AimConstraint ac = spriteRenderer.transform.parent.GetComponent<AimConstraint>();
        
        //reset everything on the AimConstraint
        ac.transform.localRotation = Quaternion.Euler(0,0,0);
        ac.constraintActive = false;
        ac.rotationAtRest = new Vector3();
        ac.rotationOffset = new Vector3();

        //revert the placement on the origin so its flat on the wall && where you actually clicked
        transform.position += Origin.transform.localPosition;
        
        //make the up vector look.. up according to the normal on the wall
        transform.rotation = Quaternion.LookRotation(normal, Vector3.up);

        //move it away from the wall a small amount to avoid z fighting 
        transform.position += normal * .1f;
    }
    

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
}
