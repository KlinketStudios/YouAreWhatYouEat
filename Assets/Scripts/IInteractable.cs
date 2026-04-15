using UnityEngine;

public interface IInteractable
{
    public void Interacted(GrabHand grabHand);

    public void InteractedWithObjectInHand(GameObject obj, GrabHand grabHand)
    {
        Interacted(grabHand);
    }
}