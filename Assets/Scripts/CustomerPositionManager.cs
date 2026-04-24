using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KlinketStudiosTools;
using UnityEngine;

public class CustomerPositionManager : MonoBehaviour
{
    [SerializeField] private GameObject[] registerPositions; 
    public bool[] registerPositionsInUse;

    [SerializeField] private GameObject[] linePositions;
    public bool[] linePositionsInUse;

    [SerializeField] private GameObject[] waitPositions;
    private bool[] waitPositionsInUse;

    [SerializeField] private GameObject servePosition;
    private bool isServePositionInUse;
    
    [SerializeField] private GameObject leavePosition;

    [SerializeField] private Event receiptTakenEvent;

    private void Awake()
    {
        receiptTakenEvent.@event += MoveCustomers;
        registerPositionsInUse = new bool[registerPositions.Length];
        waitPositionsInUse = new bool[waitPositions.Length];
        linePositionsInUse = new bool[linePositions.Length];
    }

    public bool TryGetCustomerRegisterPositionAndOccupy(out GameObject position, out int positionIndex)
    {
        for (int i = 0; i < registerPositions.Length; i++)
        {
            if (!registerPositionsInUse[i])
            {
                registerPositionsInUse[i] = true;
                position = registerPositions[i];
                positionIndex = i;
                return true;
            }
        }
        positionIndex = -1;
        position = null;
        return false;
    }
    
    public bool TryGetCustomerLinePositionAndOccupy(out GameObject position, out int positionIndex)
    {
        for (int i = 0; i < linePositions.Length; i++)
        {
            if (!linePositionsInUse[i])
            {
                linePositionsInUse[i] = true;
                position = linePositions[i];
                positionIndex = i;
                return true;
            }
        }
        positionIndex = -1;
        position = null;
        return false;
    }
    
    public bool TryGetCustomerWaitPositionAndOccupy(out GameObject position, out int positionIndex)
    {
        for (int i = 0; i < waitPositions.Length; i++)
        {
            if (!waitPositionsInUse[i])
            {
                waitPositionsInUse[i] = true;
                position = waitPositions[i];
                positionIndex = i;
                return true;
            }
        }
        positionIndex = -1;
        position = null;
        return false;
    }

    public GameObject GetLeavePositonObject()
    {
        return leavePosition;
    }
    
/// <summary>
/// get the destination of the serve location
/// </summary>
/// <param name="servePositionObject">returns the GameObject that is positioned at the serve location</param>
/// <returns>true if position is not in use and try get succeeded, returns false if position is already in use</returns>
    public bool TryGetServePositionAndOccupy(out GameObject servePositionObject)
    {
        if (isServePositionInUse)
        {
            servePositionObject = null;
            return false;
        }

        servePositionObject = servePosition;
        isServePositionInUse = true;
        return true;
    }

    public void UnoccupyRegisterPosition(int position)
    {
        registerPositionsInUse[position] = false;
    }
    public void UnoccupyWaitPosition(int position)
    {
        waitPositionsInUse[position] = false;
    }

    public void UnoccupyServePosition()
    {
        isServePositionInUse = false;
    }
    public void UnoccupyLinePosition(int position)
    {
        linePositionsInUse[position] = false;
    }
    
    
    //customer line system 

    public List<CustomerNavigation> customerRegistry = new List<CustomerNavigation>();
    
    public void RegisterCustomerToLine(CustomerNavigation cn)
    {
        customerRegistry.Add(cn);
    }

    public bool DeregisterCustomerToLine(CustomerNavigation cn)
    {
        if (customerRegistry.Remove(cn))
        {
            return true;
        }

        return false;
    }

    private async void MoveCustomers()
    {
        foreach (var cn in customerRegistry)
        {
            await Task.Yield();
            await Task.Yield();
            cn.TryMoveToRegisterPositionElseLinePosition();
        }
    }
    
}
