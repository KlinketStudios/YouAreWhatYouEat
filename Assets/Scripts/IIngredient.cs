using System.Collections.Generic;

public interface IIngredient : IInteractable, IPickupAndPlaceable, IClickListener, ITrashable
{
    public IngredientTypes Type { get; set; }
    public List<ICondiment> CondimentStack { get; set; }
    public Plate Plate { get; set; }
}