using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CustomerRequestBubble : MonoBehaviour
{
    [SerializeField] private TMP_Text requestText;
    [SerializeField] private List<RequestedIngredient> requestedIngredients;
    
    public CustomerRequest cr; 
    
    private string text;

    public void ShowRequest()
    {
        GetComponent<SpriteRenderer>().enabled = true;

        requestText.text = cr.order;
    }

    public void HideRequestBubble()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        requestText.alpha = 0;
    }
}