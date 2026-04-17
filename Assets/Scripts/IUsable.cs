using UnityEngine;

public interface IUsable
{
    public void Use(GrabHand grabHand);
    public void UsedOnObject(GrabHand grabHand, GameObject objUsedOn);
}