using UnityEngine;

public interface IClickListener 
{
    public void Click(GrabHand grabHand);
    public void ClickWithObjectInHand(GameObject obj, GrabHand grabHand);
}