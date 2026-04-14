using System;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    private GameObject objectInLeftHand;
    private GameObject objectInRightHand;
    private bool isHoldingSomethingInLeftHand;
    private bool isHoldingSomethingInRightHand;

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
}

public enum GrabHand
{
    LeftHand,
    RightHand
}
