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
/// <summary>
/// check if the player is holding an item in grabHand
/// </summary>
/// <param name="grabHand">which hand to check</param>
/// <returns>true if player is holding item, false if player is not</returns>
/// <exception cref="ArgumentOutOfRangeException">can only be reached if grabHand is an invalid enum value</exception>
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
/// <summary>
/// get what the player is holding in grabHand
/// </summary>
/// <param name="grabHand">which hand to check</param>
/// <returns>the gameobject that is being held, returns null if player is not holding anything</returns>
/// <exception cref="ArgumentOutOfRangeException">can only be reached if grabHand is an invalid enum value</exception>
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
/// <summary>
/// get the GrabPoint transform of the grabHand 
/// </summary>
/// <param name="grabHand">which hand to get grabPoint of</param>
/// <returns>the grabPoint transform of grabHand</returns>
/// <exception cref="ArgumentOutOfRangeException">can only be reached if grabHand is an invalid enum value</exception>
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
/// <summary>
/// grab object or clear grabHand
/// </summary>
/// <param name="obj">what object to grab</param>
/// <param name="grabHand">which hand to put it in</param>
/// <exception cref="ArgumentOutOfRangeException">can only be reached if grabHand is an invalid enum value</exception>
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