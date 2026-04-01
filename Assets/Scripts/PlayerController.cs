using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;
    private InputAction lookAction;
    private InputAction moveAction;
    
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float lookSpeed = 2f;
    
    private float gravity;
    private float lookAngle;
    private Vector3 moveDir;
    
    private CharacterController cc;
    private Camera camera;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = inputActions.FindAction("Move");
        lookAction = inputActions.FindAction("Look");
        cc = GetComponent<CharacterController>();
        camera = Camera.main;
    }

    public void OnEnable()
    {
        inputActions.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        moveDir = new Vector3(moveAction.ReadValue<Vector2>().x, -gravity, moveAction.ReadValue<Vector2>().y);

        moveDir = moveDir.normalized;
        
        cc.Move(transform.TransformDirection(moveDir * (moveSpeed * Time.deltaTime)));
        transform.Rotate(new Vector3(0,lookAction.ReadValue<Vector2>().x * (lookSpeed * Time.deltaTime),0));

        lookAngle = Mathf.Clamp(lookAngle + -lookAction.ReadValue<Vector2>().y * (lookSpeed * Time.deltaTime), -80, 80);

        camera.transform.localRotation = Quaternion.Euler(new Vector3(lookAngle, 0, 0));
    }

    public void OnDisable()
    {
        inputActions.Disable();
    }
}
