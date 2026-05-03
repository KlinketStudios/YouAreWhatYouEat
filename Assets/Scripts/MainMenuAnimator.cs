using System;
using UnityEngine;

public class MainMenuAnimator : MonoBehaviour
{

    [Header("BaseElements")]
    [SerializeField] private GameObject flurishes;
    
    [Header("HomePageElements")]
    [SerializeField] private GameObject homePageParent;
    [SerializeField] private GameObject title;
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject quitButton;
    [SerializeField] private GameObject settingsButton;
    [SerializeField] private GameObject creditsButton;

    [Header("SettingsElements")] 
    [SerializeField] private GameObject settingsPageParent;
    [SerializeField] private GameObject sensitivitySlider;
    [SerializeField] private GameObject FPSSlider;
    [SerializeField] private GameObject settingsBackButton;
    
    [Header("CreditsElements")]
    [SerializeField] private GameObject creditsPageParent;
    [SerializeField] private GameObject creditsBackButton;


    
    public void SlideToSettings()
    {
        FlurishLeft();
        homePageParent.GetComponent<CanvasGroup>().LeanAlpha(0, .5f).setEaseOutQuart();
        homePageParent.transform.LeanMoveLocalX(-Screen.width * 1.5f, .7f).setEaseOutQuart();
        settingsPageParent.SetActive(true);
        settingsPageParent.transform.position = new Vector3(Screen.width,settingsPageParent.transform.position.y);
        settingsPageParent.transform.LeanMoveX(0, .7f).setEaseOutBack();
    }

    public void SlideToHomeFromSettings()
    {
        FlurishRight();
        homePageParent.GetComponent<CanvasGroup>().LeanAlpha(1, .5f).setEaseOutQuart();
        homePageParent.transform.LeanMoveLocalX(0, .7f).setEaseOutBack();
        settingsPageParent.SetActive(false);
        settingsPageParent.transform.position = new Vector3(0,settingsPageParent.transform.position.y);
        settingsPageParent.transform.LeanMoveX(Screen.width, .7f).setEaseOutQuart();
    }
    
    private void FlurishLeft()
    {
        var sequence = LeanTween.sequence();
        sequence.append(flurishes.LeanScale(Vector3.one * .98f, .2f).setLoopPingPong(1).setEaseOutCubic().setDelay(0.1f));
        sequence.append(flurishes.LeanMoveLocalX(-20, .2f).setLoopPingPong(1).setEaseOutCubic().setDelay(0.1f));

    }
    
    private void FlurishRight()
    {
        var sequence = LeanTween.sequence();
        sequence.append(flurishes.LeanScale(Vector3.one * .98f, .2f).setLoopPingPong(1).setEaseOutCubic().setDelay(0.1f));
        sequence.append(flurishes.LeanMoveLocalX(20, .2f).setLoopPingPong(1).setEaseOutCubic().setDelay(0.1f));

    }
    
    public void SlideToCredits()
    {
        FlurishLeft();
        homePageParent.GetComponent<CanvasGroup>().LeanAlpha(0, .5f).setEaseOutQuart();
        homePageParent.transform.LeanMoveLocalX(-Screen.width * 1.5f, .7f).setEaseOutQuart();
        creditsPageParent.SetActive(true);
        creditsPageParent.transform.position = new Vector3(Screen.width,settingsPageParent.transform.position.y);
        creditsPageParent.transform.LeanMoveX(0, .7f).setEaseOutBack();
    }

    public void SlideToHomeFromCredits()
    {
        FlurishRight();
        homePageParent.GetComponent<CanvasGroup>().LeanAlpha(1, .5f).setEaseOutQuart();
        homePageParent.transform.LeanMoveLocalX(0, .7f).setEaseOutBack();
        creditsPageParent.SetActive(false);
        creditsPageParent.transform.position = new Vector3(0,settingsPageParent.transform.position.y);
        creditsPageParent.transform.LeanMoveX(Screen.width, .7f).setEaseOutQuart();
    }
}
