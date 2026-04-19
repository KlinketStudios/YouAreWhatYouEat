using UnityEngine;

public interface IInteractable
{
    public IClickListener ClickListener { get; set; }

    public void Interacted(GrabHand grabHand);

    public void InteractedWithObjectInHand(GameObject obj, GrabHand grabHand)
    {
        Interacted(grabHand);
    }

    public static GameObject GetRootListener(IInteractable interactable)
    {
        if (interactable.ClickListener != null)
            return GetRootListener(interactable.ClickListener.ClickListener);
        else
            return null;
    }


}