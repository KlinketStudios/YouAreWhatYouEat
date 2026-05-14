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
        //get references to all scripts regarding the customer ai
        cpm = GameObject.FindGameObjectWithTag("CustomerPositionManager").GetComponent<CustomerPositionManager>();
        agent = GetComponent<NavMeshAgent>();
        cr = GetComponent<CustomerRequest>();
        crb = cr.requestBubble;
        co = GetComponent<CustomerOrder>();
    }
    
    public void Update()
    {
        //check if is a starting frame
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
        /// <summary>
        /// when the customer is walking into the restaurant and when waiting in line
        /// </summary>
        /// <param name="enterFrame">is the starting frame of the state</param>
        private void EnteringState(bool enterFrame)
        {
            CustomerRequest thisCustomerRequest = null;
            if (enterFrame)
            {
                //register self to the customer line manager
                cpm.RegisterCustomerToLine(this);
                
                
                //check if can move to the register position
                TryMoveToRegisterPositionElseLinePosition();
            }
    
            //check if the customer has reached their destination
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                //check if order was taken
                if (wasOrderTaken)
                {
                    //move state on to requesting 
                    state = CustomerState.Requesting;
                    
                    //deregister customer from the customer line manager
                    cpm.DeregisterCustomerToLine(this);
                }
            }
        }
    
        /// <summary>
        /// when the customer is requesting their order
        /// </summary>
        /// <param name="enterFrame">is the starting frame of the state</param>
        private void RequestingState(bool enterFrame)
        {
            if (enterFrame)
            {
                //show the customers request
                crb.ShowRequest();
            }
            
            //check if the receipt was taken 
            if(receiptTaken.wasTriggeredThisFrame)
            {
                //ticket was taken from the receipt printer 
                
                //unoccupy the register position
                UnoccupyThisRegisterPosition();
                
                //move to wait position
                MoveToOpenWaitPosition();
                
                //hide request bubble
                crb.HideRequestBubble();
                
                //move state to waiting state
                state = CustomerState.Waiting;
            }
        }
        /// <summary>
        /// when the customer is waiting for their order to be made
        /// </summary>
        /// <param name="enterFrame">is the starting frame of the state</param>
        private void WaitingState(bool enterFrame)
        {
            //check if order was served
            if(orderServed.wasTriggeredThisFrame && 
               (int)orderServed.dataSlot1 == cr.orderID)
            {
                //unoccupy wait position
                UnoccupyThisWaitPosition();
                
                //move to serve position to get food
                MoveToServePosition();
                
                //move to getting food state
                state = CustomerState.GettingFood;
            }
        }
    
        /// <summary>
        /// when the customer is grabbing their order from the serve board
        /// </summary>
        /// <param name="enterFrame">is the starting frame of the state</param>
        private void GettingFoodState(bool enterFrame)
        {
            //check if the customer has judged the food already
            if (!hasJudgedFood)
            {
                //check if we have reached the serve position
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    //judge the food
                    co.JudgeFood();
                    hasJudgedFood = true;
                }
            }
            //customer has already judged their food 
            else
            {
                //wait 2 seconds 
                if ((serveWaitTimer += Time.deltaTime) >= 2)
                {
                    //reset the timer
                    //technically not needed
                    serveWaitTimer = 0;
                    //move to leaving state
                    state = CustomerState.Leaving; 
                    
                    //delete the food
                    co.TakeFood();
                }
            }
        }
    
        /// <summary>
        /// when the customer is leaving the restaurant 
        /// </summary>
        /// <param name="enterFrame">is the starting frame of the state</param>
        private void LeavingState(bool enterFrame)
        {
            //unoccupy the serve position
            cpm.UnoccupyServePosition();
            
            //move out of the resturaunt  
            agent.destination = cpm.GetLeavePositonObject().transform.position;
            
            //spawn a new customer
            if(enterFrame)
                cpm.SpawnCustomer();
                
            //destroy this customer when they reach the (leave) position
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                Destroy(gameObject);
            }
        }
        
    #endregion

/// <summary>
/// try to move to the register position, if it is already occupied move to the next available line position
/// </summary>
public void TryMoveToRegisterPositionElseLinePosition()
{
    if (!MoveToOpenRegisterPosition())
    {
        MoveToLinePosition();
    }
}

/// <summary>
/// try to move to the serve position
/// </summary>
/// <returns>true if this customer was able to move to the serve position, false if serve position was already occupied</returns>
public bool MoveToServePosition()
{
    if (cpm.TryGetServePositionAndOccupy(out GameObject positionObject))
    {
        agent.destination = positionObject.transform.position;
        return true;
    }

    return false;
}

/// <summary>
/// try to move to the next available line position
/// </summary>
/// <returns>true if found an available line position, false if it did not</returns>
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

/// <summary>
/// try to move to the register position
/// </summary>
/// <returns>true if it was able to move to the register position, false if it was not</returns>
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

/// <summary>
/// try to move to a wait position
/// </summary>
/// <returns>true if the customer was able to move to a wait position, false if it was not</returns>
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

/// <summary>
/// unoccupy the register position
/// </summary>
public void UnoccupyThisRegisterPosition()
{
    cpm.UnoccupyRegisterPosition(occupiedRegisterPosition);
    occupiedRegisterPosition = -1;
}

/// <summary>
/// onoccupy this wait position
/// </summary>
public void UnoccupyThisWaitPosition()
{
    cpm.UnoccupyWaitPosition(occupiedWaitPosition);
    occupiedWaitPosition = -1;
}

/// <summary>
/// unoccupy this line position
/// </summary>
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