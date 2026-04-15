using UnityEngine;

public class Condiment : MonoBehaviour, ICondiment
{
    public void Interacted(GrabHand grabHand)
    {
        ClickListener?.Click(grabHand);
    }

    public void InteractedWithObjectInHand(GameObject obj, GrabHand grabHand)
    {
        ClickListener?.ClickWithObjectInHand(obj, grabHand);
    }

    public IngredientTypes Type { get; set; }

    public IClickListener ClickListener { get; set; }
}