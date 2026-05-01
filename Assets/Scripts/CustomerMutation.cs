using System;
using System.Collections.Generic;
using UnityEngine;

public class CustomerMutation : MonoBehaviour
{

    private CustomerRequest cr;
    private CustomerRequestBubble crb;
    private CustomerOrder co;
    private CustomerNavigation cn;
    private CustomerPositionManager cpm;
    private CustomerMutationManager cmm;
    [SerializeField] private SpriteRenderer sr;

    private int customerAppearanceNumber;

    
    
    private int thisCustomerType;
    [SerializeField] private Sprite[] stages = new Sprite[5];
    
    
    private void Awake()
    {
        cpm = GameObject.FindGameObjectWithTag("CustomerPositionManager").GetComponent<CustomerPositionManager>();
        cmm = GameObject.FindGameObjectWithTag("CustomerMutationManager").GetComponent<CustomerMutationManager>();
        cr = GetComponent<CustomerRequest>();
        cn = GetComponent<CustomerNavigation>();
        crb = cr.requestBubble;
        co = GetComponent<CustomerOrder>();

        thisCustomerType = cmm.GetRandomCustomerType();
        stages = cmm.GetMutationSprites(thisCustomerType);
        customerAppearanceNumber = cmm.LookUpCustomerOccurenceNumber(thisCustomerType);

        InitStage();
    }

    private void InitStage()
    {
        sr.sprite = stages[customerAppearanceNumber];
    }
    
}