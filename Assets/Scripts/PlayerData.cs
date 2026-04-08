using UnityEngine;
using VHierarchy.Libs;

public class PlayerData : MonoBehaviour
{
    private GameObject objectInLeftHand;
    private GameObject objectInRightHand;
    private bool isHoldingSomethingInLeftHand;
    private bool isHoldingSomethingInRightHand;

    public bool HandedIsHolding(GrabHand grabHand)
    {
        return (bool)this.GetFieldValue($"isHoldingSomething{grabHand.ToString()}");
    }
    public GameObject HandedHeldObject(GrabHand grabHand)
    {
        return (GameObject)this.GetFieldValue($"objectIn{grabHand.ToString()}");
    }
}

public enum GrabHand
{
    LeftHand,
    RightHand
}
