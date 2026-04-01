using UnityEngine;

public class CursorManager : MonoBehaviour
{

    public void Start()
    {
        LockCursor();
    }
    
    public static void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    public static void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
    }
}
