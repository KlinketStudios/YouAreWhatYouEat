using UnityEngine;

public class InteractableTicket : MonoBehaviour, IInteractable, IPickupAndPlaceable
{
    private IClickListener clickListener;
    [SerializeField] private Transform origin;
    private GameObject thisObject;
    private int oldLayer;
    private Vector3 oldLocalScale;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        thisObject = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void Interacted(GrabHand grabHand)
    {
        GetComponent<IPickupAndPlaceable>().PickUp(grabHand);
    }

    public void Placed()
    {
        throw new System.NotImplementedException();
    }

    public void PickUp(GrabHand grabHand)
    {
        throw new System.NotImplementedException();
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
