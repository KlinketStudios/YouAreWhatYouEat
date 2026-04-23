using UnityEngine;
using UnityEngine.Animations;

public class InteractableTicket : MonoBehaviour, IInteractable, IPickupAndPlaceable
{
    private IClickListener clickListener;
    [SerializeField] private Transform origin;
    private GameObject thisObject;
    private int oldLayer;
    private Vector3 oldLocalScale;
    [SerializeField] private SpriteRenderer spriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        thisObject = gameObject;
        BoxCollider collider = spriteRenderer.GetComponent<BoxCollider>();
        collider.size = spriteRenderer.sprite.bounds.size;
    }
    
    public void Interacted(GrabHand grabHand)
    {
        GetComponent<IPickupAndPlaceable>().PickUp(grabHand);
    }

    public void Placed(Vector3 normal)
    {
        
        if (Mathf.Abs(Vector3.Dot(normal, Vector3.up)) >= .5)
            return;
        
        AimConstraint ac = spriteRenderer.transform.parent.GetComponent<AimConstraint>();
        
        ac.transform.localRotation = Quaternion.Euler(0,0,0);
        ac.constraintActive = false;
        ac.rotationAtRest = new Vector3();
        ac.rotationOffset = new Vector3();

        transform.position += Origin.transform.localPosition;
        
        transform.rotation = Quaternion.LookRotation(normal, Vector3.up);

        transform.position += normal * .1f;
    }

    public void Grabbed()
    {
        
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
}
