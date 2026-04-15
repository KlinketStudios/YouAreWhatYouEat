using System;
using UnityEngine;
using UnityEngine.Animations;

[RequireComponent(typeof(AimConstraint))]
public class LookAtMainCamera : MonoBehaviour
{
    [Space(15)]
    [SerializeField] private bool lockX;

    [SerializeField] private AimConstraint.WorldUpType worldUpType;
    
    public void Awake()
    {
        AimConstraint constraint = GetComponent<AimConstraint>();

        constraint.worldUpType = worldUpType;
        
        constraint.constraintActive = true;

        if (lockX)
        {
            constraint.rotationAxis = Axis.Y | Axis.Z;
        }
        else
        {
            constraint.rotationAxis = Axis.X | Axis.Y | Axis.Z;
        }
        
        
        constraint.AddSource(new ConstraintSource
            { sourceTransform = Camera.main.transform, weight = 1 });
    }
}