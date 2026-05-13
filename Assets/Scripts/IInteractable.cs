using UnityEngine;

public interface IInteractable
{
    public IClickListener ClickListener { get; set; }

    /// <summary>
    /// when this item was interacted with from the player either with something in grabHand or not
    /// </summary>
    /// <param name="grabHand">which hand interacted with this item</param>
    public void Interacted(GrabHand grabHand);

    /// <summary>
    /// when this item was interacted with from the player only when grabHand is already holding something
    /// </summary>
    /// <param name="obj">what object is in grabHand</param>
    /// <param name="grabHand">which hand interacted with this item</param>
    public void InteractedWithObjectInHand(GameObject obj, GrabHand grabHand)
    {
        //by default just call Interacted
        Interacted(grabHand);
    }

    //unused code
    public static GameObject GetRootListener(IInteractable interactable)
    {
        if (interactable.ClickListener != null)
            return GetRootListener(interactable.ClickListener.ClickListener);
        else
            return null;
    }


}