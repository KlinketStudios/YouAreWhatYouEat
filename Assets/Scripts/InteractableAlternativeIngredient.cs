using System;
using System.Collections.Generic;
using UnityEngine;

public class InteractableAlternativeIngredient : InteractableIngredient, IAlternativeIngredient
{
    [SerializeField, Range(0,1)] private float alternativeness;

    public float Alternativeness
    {
        get => alternativeness;
        set => alternativeness = value;
    }
}