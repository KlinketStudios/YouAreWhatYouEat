using UnityEngine;

public interface IIngredient : IInteractable, IPickupAndPlaceable, IClickListener
{
    public IngredientTypes Type { get; set; }
}