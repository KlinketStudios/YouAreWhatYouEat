using System;
using UnityEngine;

public class Knife : MonoBehaviour, IInteractable, IPickupAndPlaceable
{
    private IClickListener clickListener;
    [SerializeField] private Transform origin;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private bool placeableOnWalls;

    private void Start()
    {
        ThisObject = gameObject;
        
        //set the box collider to match the size of the sprite
        BoxCollider collider = spriteRenderer.GetComponent<BoxCollider>();
        collider.size = spriteRenderer.sprite.bounds.size;
    }

    public void Interacted(GrabHand grabHand)
    {
        //player interacted
        //pick up 
        GetComponent<IPickupAndPlaceable>().PickUp(grabHand);
    }
    
    public void Grabbed()
    {
        //hide the knife to use the drawn in hand version
        spriteRenderer.enabled = false;
    }

    public void Placed(Vector3 normal)
    {
        //re-show the knife
        spriteRenderer.enabled = true;
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

    public GameObject ThisObject { get; set; }
    public int OldLayer { get; set; }
    public Vector3 OldLocalScale { get; set; }

    public bool PlaceableOnWalls
    {
        get => placeableOnWalls;
        set => placeableOnWalls = value;
    }

    #endregion
}
