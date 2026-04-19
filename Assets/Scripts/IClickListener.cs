using UnityEngine;

public interface IClickListener : IInteractable
{
    public void Click(GrabHand grabHand);
    public void ClickWithObjectInHand(GameObject obj, GrabHand grabHand);
}