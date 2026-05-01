using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private Slider senseSlider;
    [SerializeField] private Slider FPSSlider;
    private SettingsData settingsData;
    private SaveSystem saveSystem;
    [SerializeField] private TMP_Text sensitivityValue;
    [SerializeField] private TMP_Text FPSValue;
    

    private void OnEnable()
    {
        saveSystem = FindAnyObjectByType<SaveSystem>();
        settingsData = saveSystem.settingsData;
        senseSlider.value = settingsData.sensitivity;
        FPSSlider.value = settingsData.FPS;
    }

    public void OnSenseSliderValueChanged(Single single)
    {
        settingsData.sensitivity = single;
        sensitivityValue.text = single.ToString();
    }    
    public void OnFPSSliderValueChanged(Single single)
    {
        settingsData.FPS = (int)single;
        FPSValue.text = single.ToString();
    }

    private void OnDisable()
    {
        saveSystem.Save();
    }
}