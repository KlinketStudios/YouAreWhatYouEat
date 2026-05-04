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


    private void Start()
    {
        playerData = GetComponent<PlayerData>();
    }

    private void Update()
    {
        var didHitObject = Physics.Raycast(cameraTransform.position, cameraTransform.forward,
            out var hitInfo, interactDist, interactLayerMask);

        GameObject objectHit = null;
        GameObject obj = null;

        if (didHitObject)
        {
            objectHit = hitInfo.collider.gameObject;
            obj = objectHit;

            if (objectHit.TryGetComponent(out DesiredRoot objectsRoot))
                obj = objectsRoot.root;

            if (leftInteractionAction.WasPerformedThisFrame())
                Interact(obj, hitInfo, GrabHand.LeftHand);


            else if (rightInteractionAction.WasPerformedThisFrame())
                Interact(obj, hitInfo, GrabHand.RightHand);
        }
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

        SetCrosshairSprite(hitInfo.normal, didHitObject, obj);
    }

    private void OnEnable()
    {
        rightInteractionAction = inputAction.FindAction("RightInteract");
        leftInteractionAction = inputAction.FindAction("LeftInteract");
        inputAction.Enable();
    }

    private void OnDisable()
    {
        inputAction.Disable();
    }

    //i want to make the crosshair split in half so that you can see what actions either hand can do 

    //example: right now if the player looks at a pickupable intending to pick it up but has something in their other hand it
    //         will not sow that the object in your other hand could be usable at that point in time

    //         i want it to show what is possible for both hands individually 
    public void SetCrosshairSprite(Vector3 normal, bool didHit, GameObject hitObject)
    {
        if (!didHit)
        {
            crosshairImage.sprite = defaultCrosshair;
            return;
        }

        if (hitObject.TryGetComponent(out IInteractable interactable))
        {
            crosshairImage.sprite = interactableCrosshair;
            return;
        }

        if (Vector3.Dot(normal, Vector3.up) == 1)
            if (playerData.HandedIsHolding(GrabHand.LeftHand) || playerData.HandedIsHolding(GrabHand.RightHand))
            {
                crosshairImage.sprite = placeableCrosshair;
                return;
            }

        crosshairImage.sprite = defaultCrosshair;
    }

    private void Interact(GameObject obj, RaycastHit hitInfo, GrabHand grabHand)
    {
        // clicked on and object
        var isHoldingObject = false;
        if (playerData.HandedIsHolding(grabHand))
            isHoldingObject = true;

        try
        {
            var isHoldingUsable = playerData.HandedHeldObject(grabHand).TryGetComponent(out IUsable usable);
            if (isHoldingUsable)
            {
                //is holding object
                //object held is a usable
                usable.UsedOnObject(grabHand, obj);
                return;
            }
        }
        catch (Exception)
        {
            
        }

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
    
}
