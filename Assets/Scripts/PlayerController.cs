using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;

    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float lookSpeed = 2f;

    private SaveSystem saveSystem;

    [SerializeField] private float gravity;
    private Camera camera;

    [HideInInspector] public CharacterController cc;
    private InputAction lookAction;
    private float lookAngle;
    private InputAction moveAction;
    [HideInInspector] public Vector3 moveDir;

    [HideInInspector] public bool isMoving;
    
   public void OnEnable()
    {
        //Enable Input
        inputActions.Enable();
    }
   
    private void Start()
    {
        //Get InputAction Instances
        moveAction = inputActions.FindAction("Move");
        lookAction = inputActions.FindAction("Look");
        
        //Cache Refs 
        saveSystem = FindFirstObjectByType<SaveSystem>();
        cc = GetComponent<CharacterController>();
        camera = Camera.main;
    }

    private void Update()
    {
        //Calculate Desired Move Die
        moveDir = new Vector3(moveAction.ReadValue<Vector2>().x, -gravity, moveAction.ReadValue<Vector2>().y);

        //Check If Is Trying To Move 
        isMoving = moveDir != new Vector3(0, -gravity, 0);
        
        //Normalize Move Dir To Make It So That You Dont Move Faster When Moving At A Diagonal 
        moveDir = moveDir.normalized;

        //Move
        cc.Move(transform.TransformDirection(moveDir * (moveSpeed * Time.deltaTime)));
    }

    
    //Late Update to make sure the player has already moved before doing hand and look movement
    private void LateUpdate()
    {
        
        //check if look sensitivity has changed in the settings
        if (lookSpeed != saveSystem.settingsData.sensitivity)
                 lookSpeed = saveSystem.settingsData.sensitivity;
        
        //rotate player horizontally according to Mouse || Gamepad delta
        transform.Rotate(new Vector3(0, lookAction.ReadValue<Vector2>().x * lookSpeed , 0));

        //Cache look angle for next frame and to keep it clamped between -80 and 80
        lookAngle = Mathf.Clamp(lookAngle + -lookAction.ReadValue<Vector2>().y * lookSpeed , -80, 80);

        //rotate camera vertically also according to the Mouse || Gamepad delta
        camera.transform.localRotation = Quaternion.Euler(new Vector3(lookAngle, 0, 0));
    }

 

    public void OnDisable()
    {
        //Disable Input
        inputActions.Disable();
    }
}