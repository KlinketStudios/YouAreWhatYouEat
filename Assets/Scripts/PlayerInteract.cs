using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputAction;
    private InputAction leftInteractionAction;
    private InputAction rightInteractionAction;

    private PlayerData playerData;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private LayerMask interactLayerMask;
    [SerializeField] private float interactDist;
    
    private void OnEnable()
    {
        rightInteractionAction = inputAction.FindAction("RightInteract");
        leftInteractionAction = inputAction.FindAction("LeftInteract");
        inputAction.Enable();
    }

    private void Start()
    {
        playerData = GetComponent<PlayerData>();
    }

    private void OnDisable()
    {
        inputAction.Disable();
    }

    private void Update()
    {
        bool didHitObject = Physics.Raycast(cameraTransform.position, cameraTransform.forward, 
            out RaycastHit hitInfo,interactDist, interactLayerMask);

        GameObject objectHit = null;

        if (didHitObject)
            return;
        
            
        objectHit = hitInfo.collider.gameObject;
        
        if (leftInteractionAction.WasPerformedThisFrame())
        {

            if (!playerData.HandedIsHolding(GrabHand.LeftHand))
            {
                print($"clicked {objectHit.name} with left hand with empty hand");
            }
            else
            {
                print($"clicked {objectHit.name} with left hand with something in hand");
            }
        }
        if (rightInteractionAction.WasPerformedThisFrame())
        {
            
            if (playerData.HandedIsHolding(GrabHand.RightHand))
            {
                print($"clicked {objectHit.name} with right hand with empty hand");
            }
            else
            {
                print($"clicked {objectHit.name} with right hand with something in hand");
            }

        }
    }

}

