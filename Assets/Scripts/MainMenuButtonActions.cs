using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//Play and Quit button animations
public class MainMenuButtonActions : MonoBehaviour
{

    [SerializeField] private GameObject hoverSprite;
    
    private void OnMouseEnter()
    {
        hoverSprite.SetActive(true);
    }

    private void OnMouseExit()
    {
        hoverSprite.SetActive(false);
    }

    private void OnMouseUpAsButton()
    {
        
    }

    private void OnMouseDown()
    {
        
    }
}
