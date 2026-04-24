using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using VHierarchy.Libs;
using Random = UnityEngine.Random;

public class CustomerRequest : MonoBehaviour
{
    public CustomerRequestBubble requestBubble;
    private CustomerNavigation cn; 
    private CustomerRequestBubble crb; 
    private CustomerOrder co;
    
    [SerializeField] public List<RequestedIngredient> requests = new List<RequestedIngredient>();
    private List<OrderableIngredients> requestedIngredientTypes = new List<OrderableIngredients>();
    
    [SerializeField] private AnimationCurve thisIngredientAmountWeight;
    [SerializeField] private AnimationCurve ingredientToRequestAmountWeight;
    private int ingredientsToRequest;
    private int iterations;
    
    public float tolerance;
    public float alternativenessDesire;
    
    public int orderID;
    public string order;
    
    public void Awake()
    {
        cn = GetComponent<CustomerNavigation>();
        co = GetComponent<CustomerOrder>();
        tolerance = Random.Range(.1f, .25f);
        alternativenessDesire = Random.Range(0, 1f);
        RequestFood();
    }
    
    public void RequestFood()
    {
        if (iterations > 3)
        {
            return;
        }
    
        if (ingredientsToRequest <= 0)
        {
            float randomValueForIngredientsToRequest =
                        Random.Range(0f, ingredientToRequestAmountWeight.keys[ingredientToRequestAmountWeight.length - 1].time);
            ingredientsToRequest = ingredientToRequestAmountWeight.Evaluate(randomValueForIngredientsToRequest).RoundToInt();
        }
        
        float randomValueForThisIngredientAmount =
            Random.Range(0f, thisIngredientAmountWeight.keys[thisIngredientAmountWeight.length - 1].time);
        
        
        int thisIngredientAmount = thisIngredientAmountWeight.Evaluate(randomValueForThisIngredientAmount).RoundToInt();
    
        
        
        if (ingredientsToRequest > 0)
        {
            if(ingredientsToRequest < thisIngredientAmount)
                thisIngredientAmount = ingredientsToRequest;
        }
        else
        {
            return;
        }

        OrderableIngredients requestedIngredientType = (OrderableIngredients)Enum.GetValues(typeof(OrderableIngredients)).GetValue(Random.Range(0, Enum.GetValues(typeof(OrderableIngredients)).Length));
    
        if (requestedIngredientTypes.Contains(requestedIngredientType))
        {
            iterations++;
            RequestFood();
            return;
        }
    
        RequestedIngredient requestedIngredient = 
            new RequestedIngredient { amount = thisIngredientAmount, type = requestedIngredientType };
        requestedIngredientTypes.Add(requestedIngredientType);
        requests.Add(requestedIngredient);
        ingredientsToRequest -= thisIngredientAmount;
        iterations = 0;
        if (ingredientsToRequest > 0)
        {
            RequestFood();
        }
        
        string text = "";
    
        foreach (var ingredient in requests)
        {
            if (text.IsNullOrEmpty())
                text += $"({ingredient.amount}) {ingredient.type.HumanName()}";
            else
                text += $"\n({ingredient.amount}) {ingredient.type.HumanName()}";
        }
    
        order = text;
        orderID = Random.Range(100000, 1000000);
    }
}