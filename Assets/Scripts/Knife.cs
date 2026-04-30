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
        
        BoxCollider collider = spriteRenderer.GetComponent<BoxCollider>();
        collider.size = spriteRenderer.sprite.bounds.size;
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

    public GameObject ThisObject { get; set; }
    public int OldLayer { get; set; }
    public Vector3 OldLocalScale { get; set; }

    public bool PlaceableOnWalls
    {
        get => placeableOnWalls;
        set => placeableOnWalls = value;
    }

    public void Grabbed()
    {
        spriteRenderer.enabled = false;
    }

    public void Placed(Vector3 normal)
    {
        spriteRenderer.enabled = true;
    }
}
