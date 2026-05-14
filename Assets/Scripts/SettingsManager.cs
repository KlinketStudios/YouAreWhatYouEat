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
        //cache save system and settings data 
        saveSystem = FindAnyObjectByType<SaveSystem>();
        settingsData = saveSystem.settingsData;
        //set sliders to values saved
        senseSlider.value = settingsData.sensitivity;
        FPSSlider.value = settingsData.FPS;
    }

    public void OnSenseSliderValueChanged(Single single)
    {
        //note changes to the sensitivity slider
        settingsData.sensitivity = single;
        sensitivityValue.text = single.ToString();
    }    
    public void OnFPSSliderValueChanged(Single single)
    {
        //note changes on the FPS slider 
        settingsData.FPS = (int)single;
        FPSValue.text = single.ToString();
    }

    private void OnDisable()
    {
        //save changes on settings menu close
        saveSystem.Save();
    }
}