using UnityEngine;

public interface ICuttable : IPickupAndPlaceable, IInteractable, ITrashable
{
    public int CutAmount { get; set; }
    public CuttableIngredients CuttableType { get; set; }
    public GameObject Product { get; set; }
    public int CurrentCut { get; set; }

}