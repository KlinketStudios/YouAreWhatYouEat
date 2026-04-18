using System;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Condiment : MonoBehaviour, ICondiment
{

    public IngredientTypes type;
    private IClickListener clickListener;
    public IIngredient ingredientOn;
    private GameObject thisObject;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    private void Awake()
    {
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
        BoxCollider collider = spriteRenderer.GetComponent<BoxCollider>();
        
        collider.size = spriteRenderer.sprite.bounds.size;
        thisObject = gameObject;
    }

    private void Start()
    {
        
    }

    public void Interacted(GrabHand grabHand)
    {
        ClickListener?.Click(grabHand);
    }

    public void InteractedWithObjectInHand(GameObject obj, GrabHand grabHand)
    {
        ClickListener?.ClickWithObjectInHand(obj, grabHand);
    }

    public IngredientTypes Type
    {
        get => type;
        set => type = value;
    }

    public IClickListener ClickListener
    {
        get => clickListener;
        set => clickListener = value;
    }

    public IIngredient IngredientOn
    {
        get => ingredientOn;
        set => ingredientOn = value;
    }

    public GameObject ThisObject
    {
        get => thisObject;
        set => thisObject = value;
    }
    
}
