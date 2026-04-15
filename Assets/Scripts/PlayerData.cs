using System;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    [HideInInspector] public GameObject objectInLeftHand;
    [HideInInspector] public GameObject objectInRightHand;
    [HideInInspector] public bool isHoldingSomethingInLeftHand;
    [HideInInspector] public bool isHoldingSomethingInRightHand;

    public Transform rightHandGrabPoint;
    public Transform leftHandGrabPoint;

    public string grabLayer;
    
    public bool HandedIsHolding(GrabHand grabHand)
    {
        switch (grabHand)
        {
            case GrabHand.LeftHand:
                return isHoldingSomethingInLeftHand;
            case GrabHand.RightHand:
                return isHoldingSomethingInRightHand;
            default:
                throw new ArgumentOutOfRangeException(nameof(grabHand), grabHand, null);
        }
    }
    public GameObject HandedHeldObject(GrabHand grabHand)
    {

        switch (grabHand)
        {
            case GrabHand.LeftHand:
                return objectInLeftHand;
            case GrabHand.RightHand:
                return objectInRightHand;
            default:
                throw new ArgumentOutOfRangeException(nameof(grabHand), grabHand, null);
        }
    }

    public Transform HandedGrabPoint(GrabHand grabHand)
    {
        switch (grabHand)
        {
            case GrabHand.LeftHand:
                return leftHandGrabPoint;
            case GrabHand.RightHand:
                return rightHandGrabPoint;
            default:
                throw new ArgumentOutOfRangeException(nameof(grabHand), grabHand, null);
        }
    }
    
    public void HandedSetObjectInHand(GameObject obj, GrabHand grabHand)
    {
        switch (grabHand)
        {
            case GrabHand.LeftHand:
                objectInLeftHand = obj;
                if (obj != null)
                    isHoldingSomethingInLeftHand = true;
                else
                    isHoldingSomethingInLeftHand = false;
                break;
            case GrabHand.RightHand:
                objectInRightHand = obj;
                if (obj != null)
                    isHoldingSomethingInRightHand = true;
                else
                    isHoldingSomethingInRightHand = false;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(grabHand), grabHand, null);
        }
    }
}


public enum GrabHand
{
    LeftHand,
    RightHand
}
