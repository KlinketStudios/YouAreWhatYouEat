using System;
using UnityEngine;
using UnityEngine.AI;

public class CustomerNavigation : MonoBehaviour
{

    private NavMeshAgent agent;
    private CustomerRequest cr;
    private CustomerRequestBubble crb;
    private CustomerOrder co;
    private CustomerPositionManager cpm;

    [HideInInspector] public int occupiedRegisterPosition = -1;
    public int occupiedLinePosition = -1;
    public int occupiedWaitPosition = -1;
    
    public CustomerState state = CustomerState.Entering;
    private CustomerState stateLastFrame = CustomerState.None;
    
    private float serveWaitTimer;
    private bool hasJudgedFood;
    
    [SerializeField] private Event orderServed;
    [SerializeField] private Event receiptTaken;
    
    [HideInInspector] public bool wasOrderTaken;
    [HideInInspector] public bool wasOrderServed;
    [HideInInspector] public bool hasRequested;
    
    private bool isEnterFrame = true;
 

    

    private void Awake()
    {
        cpm = GameObject.FindGameObjectWithTag("CustomerPositionManager").GetComponent<CustomerPositionManager>();
        agent = GetComponent<NavMeshAgent>();
        cr = GetComponent<CustomerRequest>();
        crb = cr.requestBubble;
        co = GetComponent<CustomerOrder>();
    }
    
    public void Update()
    {
    
        if (stateLastFrame != state)
            isEnterFrame = true;
        else
            isEnterFrame = false;
        stateLastFrame = state;
        switch (state)
        {
            case CustomerState.Entering:
                EnteringState(isEnterFrame);
                break;
            case CustomerState.Requesting:
                RequestingState(isEnterFrame);
                break;
            case CustomerState.Waiting:
                WaitingState(isEnterFrame);
                break;
            case CustomerState.GettingFood:
                GettingFoodState(isEnterFrame);
                break;
            case CustomerState.Leaving:
                LeavingState(isEnterFrame);
                break;
        }
    
    }
    
    #region States
        private void EnteringState(bool enterFrame)
        {
        
            CustomerRequest thisCustomerRequest = null;
            if (enterFrame)
            {
                cpm.RegisterCustomerToLine(this);
                TryMoveToRegisterPositionElseLinePosition();
            }
    
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (wasOrderTaken)
                {
                    state = CustomerState.Requesting;
                    cpm.DeregisterCustomerToLine(this);
                }
            }
        }
    
    
        private void RequestingState(bool enterFrame)
        {
            if (enterFrame)
            {
                crb.ShowRequest();
            }
            if(receiptTaken.wasTriggeredThisFrame)
            {
                UnoccupyThisRegisterPosition();
                MoveToOpenWaitPosition();
                crb.HideRequestBubble();
                state = CustomerState.Waiting;
            }
        }
        
        private void WaitingState(bool enterFrame)
        {
                
            if(orderServed.wasTriggeredThisFrame && 
               (int)orderServed.dataSlot1 == cr.orderID)
            {
                UnoccupyThisWaitPosition();
                MoveToServePosition();
                state = CustomerState.GettingFood;
            }
        }
    
        private void GettingFoodState(bool enterFrame)
        {
            if (!hasJudgedFood)
            {
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    //Finns Code
                    //else:rat meat
    
                    co.JudgeFood();
                    hasJudgedFood = true;
                }
            }
            else
            {
                if ((serveWaitTimer += Time.deltaTime) >= 2)
                {
                    serveWaitTimer = 0;
                    state = CustomerState.Leaving; 
                    co.TakeFood();
                }
            }
        }
    
        private void LeavingState(bool enterFrame)
        {
            cpm.UnoccupyServePosition();
            agent.destination = cpm.GetLeavePositonObject().transform.position;
        }
        
    #endregion

public void TryMoveToRegisterPositionElseLinePosition()
{
    if (!MoveToOpenRegisterPosition())
    {
        MoveToLinePosition();
    }
}

public bool MoveToServePosition()
{
    if (cpm.TryGetServePositionAndOccupy(out GameObject positionObject))
    {
        agent.destination = positionObject.transform.position;
        return true;
    }

    return false;
}

public bool MoveToLinePosition()
{
    
    if (cpm.TryGetCustomerLinePositionAndOccupy(out GameObject positionObject, out int i))
    {
        if (occupiedLinePosition != -1) 
        {
            UnoccupyThisLinePosition();
        }
        agent.destination = positionObject.transform.position;
        occupiedLinePosition = i;
        return true;
    }

    return false;
}

public bool MoveToOpenRegisterPosition()
{
    if (cpm.TryGetCustomerRegisterPositionAndOccupy(out GameObject positionObject, out int i))
    {
        if (occupiedLinePosition != -1) 
        {
            UnoccupyThisLinePosition();
        }
        agent.SetDestination(positionObject.transform.position);
        occupiedRegisterPosition = i;
        return true;
    }

    return false;
}

public bool MoveToOpenWaitPosition()
{
    if (cpm.TryGetCustomerWaitPositionAndOccupy(out GameObject positionObject, out int i))
    {
        agent.SetDestination(positionObject.transform.position);
        occupiedWaitPosition = i;
        return true;
    }

    return false;
}

public void UnoccupyThisRegisterPosition()
{
    cpm.UnoccupyRegisterPosition(occupiedRegisterPosition);
    occupiedRegisterPosition = -1;
}

public void UnoccupyThisWaitPosition()
{
    cpm.UnoccupyWaitPosition(occupiedWaitPosition);
    occupiedWaitPosition = -1;
}
public void UnoccupyThisLinePosition()
{
    cpm.UnoccupyLinePosition(occupiedLinePosition);
    occupiedLinePosition = -1;
}

public enum CustomerState
{
    None,
    Entering,
    Requesting,
    Waiting,
    GettingFood,
    Leaving
}
}