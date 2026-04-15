using UnityEngine;

public interface IUsable
{
    public void Use(GrabHand grabHand, bool usedOnObject, bool usedOnInteractable, GameObject objUsedOn);
}