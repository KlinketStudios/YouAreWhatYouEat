using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputAction;
    [SerializeField] public Transform cameraTransform;
    [SerializeField] public LayerMask interactLayerMask;
    [SerializeField] public float interactDist;

    [SerializeField] private Sprite defaultCrosshair,
        interactableCrosshair,
        placeableCrosshair;

    [SerializeField] private Image crosshairImage;

    private PlayerData playerData;
    private InputAction leftInteractionAction;
    private InputAction rightInteractionAction;

    private void OnEnable()
    {
        //Enable Input
        inputAction.Enable();
        
        //Get InputAction instances
        rightInteractionAction = inputAction.FindAction("RightInteract");
        leftInteractionAction = inputAction.FindAction("LeftInteract");
    }
    private void Start()
    {
        //Get References
        playerData = GetComponent<PlayerData>();
    }

    private void Update()
    {
        //Shoot raycast to see what the player is looking at
        var didHitObject = Physics.Raycast(cameraTransform.position, cameraTransform.forward,
            out var hitInfo, interactDist, interactLayerMask);

        GameObject objectHit = null;
        GameObject obj = null;

        //if player is looking at an object
        if (didHitObject)
        {
            //cache object hit for this frame 
            objectHit = hitInfo.collider.gameObject;
            obj = objectHit;

            //check if object hit has a desired root object and set hit object to that instead
            if (objectHit.TryGetComponent(out DesiredRoot objectsRoot))
                obj = objectsRoot.root;

            //Player interacted with left hand 
            if (leftInteractionAction.WasPerformedThisFrame())
                Interact(obj, hitInfo, GrabHand.LeftHand);


            //Player interacted with right hand 
            else if (rightInteractionAction.WasPerformedThisFrame())
                Interact(obj, hitInfo, GrabHand.RightHand);
        }
        //Player is not looking at an object
        else
        {
            //not looking at object
            if (leftInteractionAction.WasPerformedThisFrame())
            {
                //Clicked
                if (playerData.HandedIsHolding(GrabHand.LeftHand))
                    //is holding object
                    if (playerData.HandedHeldObject(GrabHand.LeftHand).TryGetComponent(out IUsable heldUsable))
                        //held object is usable
                        heldUsable.Use(GrabHand.LeftHand);
            }
            else if (rightInteractionAction.WasPerformedThisFrame())
                //Clicked
                if (playerData.HandedIsHolding(GrabHand.RightHand))
                    //is holding object
                    if (playerData.HandedHeldObject(GrabHand.RightHand).TryGetComponent(out IUsable heldUsable))
                        //held object is usable
                        heldUsable.Use(GrabHand.RightHand);
        }

        //set crosshair sprite according to what player is looking at 
        SetCrosshairSprite(hitInfo.normal, didHitObject, obj);
    }
    
    
    public void SetCrosshairSprite(Vector3 normal, bool didHit, GameObject hitObject)
    {
        if (!didHit)
        {
            //not looking at anything, use default crosshair
            crosshairImage.sprite = defaultCrosshair;
            return;
        }

        if (hitObject.TryGetComponent(out IInteractable interactable))
        {
            //looking at interactable, use interactable crosshair
            crosshairImage.sprite = interactableCrosshair;
            return;
        }

        if (Vector3.Dot(normal, Vector3.up) == 1)
            if (playerData.HandedIsHolding(GrabHand.LeftHand) || playerData.HandedIsHolding(GrabHand.RightHand))
            {
                //looking at placeable location && player has something in their hand, use placeable crosshair
                crosshairImage.sprite = placeableCrosshair;
                return;
            }

        //default catch, use default crosshair
        crosshairImage.sprite = defaultCrosshair;
    }

    private void Interact(GameObject obj, RaycastHit hitInfo, GrabHand grabHand)
    {
        //clicked on and object
        
        //cache is holding in respective hand
        var isHoldingObject = false;
        if (playerData.HandedIsHolding(grabHand))
            isHoldingObject = true;

        //check if player is trying to use an object
        var isHoldingUsable = playerData.HandedHeldObject(grabHand).TryGetComponent(out IUsable usable);
        if (isHoldingUsable)
        {
            //tell held usable it was used 
            usable.UsedOnObject(grabHand, obj);
            return;
        }

        //check if interacted with interactable 
        if (obj.TryGetComponent(out IInteractable interactable))
        {
            //might have object in hand
            //object clicked on is interactable
            if (isHoldingObject)
                //is holding object
                interactable.InteractedWithObjectInHand(playerData.HandedHeldObject(grabHand), grabHand);
            else
                //is not holding object
                interactable.Interacted(grabHand);
        }
        else if (isHoldingObject)
        {
            //did not click on interactable
            //is holding object
            playerData.HandedHeldObject(grabHand).GetComponent<IPickupAndPlaceable>()
                .PutDownAtLookPoint(grabHand);
        }
    }
    
    private void OnDisable()
    {
        //Disable Input
        inputAction.Disable();
    }
}
