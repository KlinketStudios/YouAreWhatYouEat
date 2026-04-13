using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputAction;
    private InputAction interactionAction;

    private PlayerData playerData;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private LayerMask interactLayerMask;
    [SerializeField] private float interactDist;
    
    private void OnEnable()
    {
        interactionAction = inputAction.FindAction("Interact");
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
        /*bool didHitObject = Physics.Raycast(cameraTransform.position, cameraTransform.forward, 
            out RaycastHit hitInfo,interactDist, interactLayerMask);

        GameObject objectHit = hitInfo.collider.gameObject;
        
        if (interactionAction.WasPerformedThisFrame())
        {
            print($"clicked {objectHit.name}");
            
            
        }*/
    }
}

