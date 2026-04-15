using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;

    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float lookSpeed = 2f;

    [SerializeField] private float gravity;
    private Camera camera;

    private CharacterController cc;
    private InputAction lookAction;
    private float lookAngle;
    private InputAction moveAction;
    private Vector3 moveDir;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        moveAction = inputActions.FindAction("Move");
        lookAction = inputActions.FindAction("Look");
        cc = GetComponent<CharacterController>();
        camera = Camera.main;
    }

    // Update is called once per frame
    private void Update()
    {
        moveDir = new Vector3(moveAction.ReadValue<Vector2>().x, -gravity, moveAction.ReadValue<Vector2>().y);

        moveDir = moveDir.normalized;

        cc.Move(transform.TransformDirection(moveDir * (moveSpeed * Time.deltaTime)));
        transform.Rotate(new Vector3(0, lookAction.ReadValue<Vector2>().x * (lookSpeed * Time.deltaTime), 0));

        lookAngle = Mathf.Clamp(lookAngle + -lookAction.ReadValue<Vector2>().y * (lookSpeed * Time.deltaTime), -80, 80);

        camera.transform.localRotation = Quaternion.Euler(new Vector3(lookAngle, 0, 0));
    }

    public void OnEnable()
    {
        inputActions.Enable();
    }

    public void OnDisable()
    {
        inputActions.Disable();
    }
}