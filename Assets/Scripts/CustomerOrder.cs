using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CustomerOrder : MonoBehaviour, IInteractable
{

    private CustomerNavigation cn;
    private CustomerRequest cr; 
    private CustomerRequestBubble crb; 
    
    [SerializeField] private Event serveOrderEvent;
    [SerializeField] private Event receiptTakenEvent;
    [SerializeField] private Event spawnReceiptEvent;
    
    [HideInInspector] public GameObject ourOrderedFoodObject;
    private IClickListener clickListener;

    public void Awake()
    {
        serveOrderEvent.@event += CheckIfServedOrderIsOurs;
        cr = GetComponent<CustomerRequest>();
        crb = cr.requestBubble;
        cn = GetComponent<CustomerNavigation>();
    }
    
    private void CheckIfServedOrderIsOurs()
    {
        if ((int)serveOrderEvent.dataSlot1 == cr.orderID)
        {
            ourOrderedFoodObject = (GameObject)serveOrderEvent.dataSlot2;
        }
    }

    public IClickListener ClickListener
    {
        get => clickListener;
        set => clickListener = value;
    }

    public void Interacted(GrabHand grabHand)
    {
        //Take order
        if (cn.state == CustomerNavigation.CustomerState.Entering &&
            cn.occupiedRegisterPosition != -1)
        {
            spawnReceiptEvent.dataSlot1 = cr.orderID;
            spawnReceiptEvent.dataSlot2 = cr.order;
            
            spawnReceiptEvent.@event.Invoke();
            
            cn.wasOrderTaken = true;
            crb.ShowRequest();
            print("Tolerance: " + cr.tolerance);
            print("Desire: " + cr.alternativenessDesire);
        }
    }
    
    public void JudgeFood()
    {
        List<IIngredient> tempList = new List<IIngredient>((List<IIngredient>)serveOrderEvent.dataSlot3);
        List<RequestedIngredient> requestedTypes = new List<RequestedIngredient>(cr.requests);
    
        foreach (var ingredient in tempList)
        {
            print($"init {ingredient.Type.HumanName()}");
        }
        
        int falseCounts = 0;
        int correctCounts = 0;
        foreach (var ingredientRequested in requestedTypes)
        {
            for (int i = 0; i < ingredientRequested.amount; i++)
            {
                bool foundIngredient = false;
                for (int x = tempList.Count - 1; x >= 0; x-- )
                {
                    var ingredient = tempList[x];
    
                    if (x == ((List<IIngredient>)serveOrderEvent.dataSlot3).Count - 1 || x == 0)
                    {
                        print($"skipped ingredient {ingredient.Type.HumanName()}");
                        continue;
                    }
                    
                    if ((int)ingredient.Type == (int)ingredientRequested.type)
                    {
                        correctCounts++;
                        print("+1 yes: " + ingredient.Type.HumanName());
                        tempList.RemoveAt(x);
                        foundIngredient = true;
                        break;
                    }
                    
                }
    
                if (!foundIngredient)
                {
                    falseCounts++;
                    print("-1 no: " + ingredientRequested.type.HumanName());
                }
            }
        }
    
        int z = 0;
        foreach (var ingredient in tempList)
        {
            if (ingredient.Type == IngredientTypes.BreadSlice)
            {
                print($"skipped ingredient {ingredient.Type.HumanName()}");
                continue;
            }
            falseCounts++;
            print("-1 extra: " + ingredient.Type.HumanName());
            z++;
        }
    
        float foodScore = (correctCounts - (float)falseCounts) / (float)correctCounts;
        //float alternativeScore = GetFoodAlternativenessModifier();
        
        
        print("food score: " + foodScore);
        //print("alternative score: " + alternativeScore);
        //print("total score: " + (foodScore + alternativeScore));
    }
    
    //private float GetFoodAlternativenessModifier()
    //{
    //    float tolerance = cr.tolerance;
    //    float alternativenessDesire = cr.alternativenessDesire;
    //
    //    float positiveToleranceRange = alternativenessDesire + tolerance;
    //    float negativeToleranceRange = alternativenessDesire - tolerance;
    //    
    //    float foodAlternativeness =
    //        (serveOrderEvent.dataSlot2 as GameObject).GetComponent<Plate>().alternativeness;
    //    
    //    if (foodAlternativeness > positiveToleranceRange)
    //    {
    //        // food alternativeness over tolerance
    //        float a = positiveToleranceRange + .5f;
    //        float b = foodAlternativeness - positiveToleranceRange;
    //
    //        return -(b / (a - positiveToleranceRange));
    //    }
    //    else if (foodAlternativeness < negativeToleranceRange)
    //    {
    //        //food alternativeness under tolerance 
    //        float a = negativeToleranceRange - .5f;
    //        float b = foodAlternativeness - a;
    //        return -(b / (negativeToleranceRange - b));
    //    }
    //    else if (Mathf.Abs(foodAlternativeness - alternativenessDesire) < tolerance)
    //    {
    //        //food alternativeness within tolerance rance
    //        if (foodAlternativeness <= positiveToleranceRange && foodAlternativeness > alternativenessDesire)
    //        {
    //            //within tolerance but above alternativeness desire
    //            float a = tolerance;
    //            float b = foodAlternativeness - alternativenessDesire;
    //
    //            return b / a;
    //        }
    //        else if (foodAlternativeness >= negativeToleranceRange && foodAlternativeness < alternativenessDesire)
    //        {
    //            //within tolerance but under alternativeness desire
    //            float a = tolerance;
    //            float b = foodAlternativeness - negativeToleranceRange;
    //
    //            return b / a;
    //        }
    //        else if (foodAlternativeness == alternativenessDesire)
    //        {
    //            // tolerance is equal to alternativeness desire
    //            return 1;
    //        }
    //    }
    //    //can never be reached
    //    return 0;
    //}
    
    
    
    public void TakeFood()
    {
        Destroy((GameObject)serveOrderEvent.dataSlot2);
    }
}
