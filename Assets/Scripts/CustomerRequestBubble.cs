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
        //show the request bubble sprite
        GetComponent<SpriteRenderer>().enabled = true;

        //show the order text
        requestText.text = cr.order;
    }

    public void HideRequestBubble()
    {
        //hide the request bubble sprite
        GetComponent<SpriteRenderer>().enabled = false;
        //hide the order text
        requestText.alpha = 0;
    }
}