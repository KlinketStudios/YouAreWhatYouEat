using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CustomerMutationManager : MonoBehaviour
{
    private List<int> customerOccurences;

    private DayManager dayManager;
    private int customersSpawnedThisDay;
    
    private List<int> availableCustomerTypesForDay = new List<int>{0,2,3,4};

    private (int, int) lockedCustomer1 = (1, 4);
    private (int, int) lockedCustomer2 = (5, 2);

    [SerializeField] private Sprite[] customer1Stages = new Sprite[5];  
    [SerializeField] private Sprite[] customer2Stages = new Sprite[5];  
    [SerializeField] private Sprite[] customer3Stages = new Sprite[5];  
    [SerializeField] private Sprite[] customer4Stages = new Sprite[5];  
    [SerializeField] private Sprite[] customer5Stages = new Sprite[5];  
    [SerializeField] private Sprite[] customer6Stages = new Sprite[5];  

    [SerializeField] private Event dayStart;

    private void Start()
    {
        customerOccurences = SaveSystem.instance.gameData.customerOccurences;
        dayManager = FindAnyObjectByType<DayManager>();
        dayStart.@event += DayStarted;
        
    }

    private void DayStarted()
    {
        availableCustomerTypesForDay = new List<int>{0,2,3,4};
        
        if (dayManager.currentDay >= lockedCustomer1.Item2)
        {
            availableCustomerTypesForDay.Add(lockedCustomer1.Item2);
        }
        if (dayManager.currentDay >= lockedCustomer2.Item2)
        {
            availableCustomerTypesForDay.Add(lockedCustomer2.Item1);
        }
    }

    public void CustomerSpawned()
    {
        customersSpawnedThisDay++;
    }

    private void LateUpdate()
    {
        if (SaveSystem.instance.gameData.customerOccurences != customerOccurences)
            SaveSystem.instance.gameData.customerOccurences = customerOccurences;
        
        if(SaveSystem.instance.gameData.customersSpawnedThisDay != customersSpawnedThisDay)
            SaveSystem.instance.gameData.customersSpawnedThisDay = customersSpawnedThisDay;
        
    }

    public Sprite[] GetMutationSprites(int type)
    {
        switch (type)
        {
            case 0:
                return customer1Stages;
            case 1:
                return customer2Stages;
            case 2:
                return customer3Stages;
            case 3:
                return customer4Stages;
            case 4:
                return customer5Stages;
            case 5:
                return customer6Stages;
            default:
                return null;
        }
    }

    public int LookUpCustomerOccurenceNumber(int type)
    {
        print(type);
        return customerOccurences[type];
    }

    public int GetRandomCustomerType()
    {
        int randIndex = Random.Range(0, availableCustomerTypesForDay.Count);
        int randType = availableCustomerTypesForDay[randIndex];
        availableCustomerTypesForDay.RemoveAt(randIndex);

        return randType;
    }
}