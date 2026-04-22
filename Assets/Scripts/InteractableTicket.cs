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
        AimConstraint ac = spriteRenderer.transform.parent.GetComponent<AimConstraint>();
        
        ac.transform.rotation = Quaternion.identity;
        ac.constraintActive = false;
        ac.rotationAtRest = new Vector3();
        ac.rotationOffset = new Vector3();

        transform.position += Origin.transform.localPosition;
        
        print(Vector3.Dot(normal, Vector3.up));
        if (Vector3.Dot(normal, Vector3.up) >= -.5 && Vector3.Dot(normal, Vector3.up) <= .5f)
        {
            
            transform.rotation = Quaternion.LookRotation(normal, Vector3.up);
            Vector3 offset = transform.rotation.eulerAngles;
                //checkpoint
                //make ticket face the wall normal have the up go in the correct direction 
                //test current code first
            transform.rotation = Quaternion.FromToRotation(offset - Quaternion.LookRotation(normal).eulerAngles, Vector3.up);
        }
        else
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            Vector3 thisPositionOnPlane = Vector3.ProjectOnPlane(transform.position, Vector3.up);
            Vector3 playerPositionOnPlane = Vector3.ProjectOnPlane(player.transform.position, Vector3.up);
            transform.rotation = Quaternion.LookRotation(normal, (thisPositionOnPlane - playerPositionOnPlane) * 2);
        }
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
