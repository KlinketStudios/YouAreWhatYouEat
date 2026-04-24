using System;

[Serializable]
public struct RequestedIngredient
{
    public OrderableIngredients type;
    public int amount;
}
