using UnityEngine;

public class CuttableIngredient : ICuttable, IPickupAndPlaceable
{
    public int cutAmount;
    public GameObject product;
    public CuttableIngredients cuttableType;
    public Transform origin;
    private GameObject thisObject;
    private int oldLayer;
    private Vector3 oldLocalScale;

    public int CutAmount
    {
        get => cutAmount;
        set => cutAmount = value;
    }

    public CuttableIngredients CuttableType
    {
        get => cuttableType;
        set => cuttableType = value;
    }

    public GameObject Product
    {
        get => product;
        set => product = value;
    }

    public Transform Origin
    {
        get => origin;
        set => origin = value;
    }

    public GameObject ThisObject
    {
        get => thisObject;
        set => thisObject = value;
    }

    public int OldLayer
    {
        get => oldLayer;
        set => oldLayer = value;
    }

    public Vector3 OldLocalScale
    {
        get => oldLocalScale;
        set => oldLocalScale = value;
    }
}