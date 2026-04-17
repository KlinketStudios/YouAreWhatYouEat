public interface ICondiment : IInteractable
{
    public IngredientTypes Type { get; set; }
    public IClickListener ClickListener { get; set; }
    public IIngredient IngredientOn { get; set; }
}