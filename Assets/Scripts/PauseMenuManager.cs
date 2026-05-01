using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{

    [SerializeField] private InputActionAsset input;
    private InputAction escAction;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        escAction = input.FindAction("Pause");
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }


    private void Update()
    {
        if (escAction.WasCompletedThisFrame())
        {
            SceneManager.LoadScene("MainMenu");
            Cursor.lockState = CursorLockMode.None;

        }
    }
}
