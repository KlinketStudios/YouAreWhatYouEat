using UnityEngine;
using UnityEngine.Animations;

//require AimConstraint component
[RequireComponent(typeof(AimConstraint))]
public class LookAtMainCamera : MonoBehaviour
{
    [Space(15)] [SerializeField] private bool lockX;

    [SerializeField] private AimConstraint.WorldUpType worldUpType;

    public void Awake()
    {
        // cache AimConstraint component
        var constraint = GetComponent<AimConstraint>();

        //set world up
        constraint.worldUpType = worldUpType;
        
        //turn on the constraint
        constraint.constraintActive = true;

        //set the axis to rotate on
        if (lockX)
            constraint.rotationAxis = Axis.Y | Axis.Z;
        else
            constraint.rotationAxis = Axis.X | Axis.Y | Axis.Z;

        //add the main camera as the object to look at
        constraint.AddSource(new ConstraintSource
            { sourceTransform = Camera.main.transform, weight = 1 });
    }
}