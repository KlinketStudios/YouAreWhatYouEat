using TMPro;
using UnityEngine;

public class FrameRateLock : MonoBehaviour
{
    private TMP_Text text;
    private SaveSystem saveSystem;
    
    void Start()
    {
        //get the reference to the save system 
        saveSystem = FindAnyObjectByType<SaveSystem>();
        //set the target frame rate to the value saved in the settings
        Application.targetFrameRate = saveSystem.settingsData.FPS;
        //cache the text component
        text = GetComponent<TMP_Text>();
    }

    void Update()
    {
        //set the text to the current frame rate
        text.text = (1 / Time.deltaTime).ToString("N0");
    }
}
