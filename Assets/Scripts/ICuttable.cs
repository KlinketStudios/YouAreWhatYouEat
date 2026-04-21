using UnityEngine;

public interface ICuttable
{
    public int CutAmount { get; set; }
    public CuttableIngredients CuttableType { get; set; }
    public GameObject Product { get; set; }
}