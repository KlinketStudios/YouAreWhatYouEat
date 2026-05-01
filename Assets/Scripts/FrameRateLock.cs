using TMPro;
using UnityEngine;

public class FrameRateLock : MonoBehaviour
{
    private TMP_Text text;
    private SaveSystem saveSystem;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        saveSystem = FindAnyObjectByType<SaveSystem>();
        Application.targetFrameRate = saveSystem.settingsData.FPS;
        text = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = (1 / Time.deltaTime).ToString("N0");
    }
}
