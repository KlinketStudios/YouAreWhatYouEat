using System.Threading.Tasks;
using KlinketStudiosTools.Utils;
using UnityEngine;
using UnityEngine.Animations;

public interface IPickupAndPlaceable
{
    public Transform Origin { get; set; }
    public GameObject ThisObject { get; set; }
    public int OldLayer { get; set; }
    public Vector3 OldLocalScale { get; set; }
    public bool PlaceableOnWalls { get; set; }
    
/// <summary>
/// pick up this object in grabHand
/// </summary>
/// <param name="grabHand">which hand to hold this object in</param>
    public void PickUp(GrabHand grabHand)
    {
        //find playerdata while avoiding FindGameObjectOfType because its very expensive 
        var playerData = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>();
        
        //check if player is holding something already
        if (playerData.HandedIsHolding(grabHand))
            return;
        
        //set the object in hand to be this object
        playerData.HandedSetObjectInHand(ThisObject, grabHand);

        //cache the grabPoint of grabHand
        var grabPoint = playerData.HandedGrabPoint(grabHand);

        //put the item in the hand visually
        ThisObject.transform.position = grabPoint.position;
        ThisObject.transform.parent = grabPoint;
        
        //set old layer so that when the player places the object it can remember what layer it was on
        OldLayer = ThisObject.layer;
        
        //set old scale so that when the player places the object it can remember the scale it was
        OldLocalScale = ThisObject.transform.lossyScale;
        
        //set scale to be the held object scale 
        ThisObject.transform.localScale = grabPoint.localScale;
        
        //set all objects childed to the held object to be on layer HeldObject
        Utils.SetLayerRecursively(ThisObject, LayerMask.NameToLayer(playerData.grabLayer));

        //make the item stop looking at the main camera 
        SetConstraintsActive(false);
        
        //call grabbed so the object know it was grabbed 
        Grabbed();
    }

/// <summary>
/// put this object down
/// </summary>
/// <param name="placePoint">where to put this object down at</param>
/// <param name="placePointNormal">which direction should up face</param>
/// <param name="grabHand">which hand this item is in</param>
    public void PutDown(Vector3 placePoint, Vector3 placePointNormal, GrabHand grabHand)
    {
        //find playerdata while avoiding FindGameObjectOfType because its very expensive 
        var playerData = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>();

        //set the object in hand to be null
        playerData.HandedSetObjectInHand(null, grabHand);

        //place object at desired position and rotation
        ThisObject.transform.position = placePoint - Origin.localPosition;
        ThisObject.transform.up = placePointNormal;
        
        //unparent object
        ThisObject.transform.parent = null;
        
        //set the scale to be the old scale 
        ThisObject.transform.localScale = OldLocalScale;
        //reset old scale
        OldLocalScale = Vector3.zero;
        
        //set all child objects and this object to be on old layer
        Utils.SetLayerRecursively(ThisObject, OldLayer);

        //make the item look at main camera agian
        SetConstraintsActive(true);
        
        //call placed so the object knows it was placed
        Placed(placePointNormal);
    }

/// <summary>
/// put this object down where the player is looking
/// </summary>
/// <param name="grabHand">which hand object is in</param>
    public void PutDownAtLookPoint(GrabHand grabHand)
    {
        //find playerdata and playerInteract while avoiding FindGameObjectOfType because its very expensive 
        var playerData = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>();
        var playerInteract = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInteract>();
        
        //shoot a ray where player is looking
        var didHitObject = Physics.Raycast(playerInteract.cameraTransform.position, playerInteract.cameraTransform.forward,
            out var hitInfo, playerInteract.interactDist, playerInteract.interactLayerMask);

        //check if this object is placeable on walls
        if (PlaceableOnWalls)
        {
            //check if object is not being placed on a flat surface
            if (Vector3.Dot(hitInfo.normal, Vector3.up) != 1)
            {
                //check if the object is being placed on a near flat wall
                if (Mathf.Abs(Vector3.Dot(hitInfo.normal, Vector3.up)) <= 0.25f)
                {
                    //continue
                }
                else
                {
                    return;
                }
            }
            else
            {
                //continue
            }
        }
        //object is not placeable on walls
        else
        {
            //check if player is not trying to place object on a wall
            if (Vector3.Dot(hitInfo.normal, Vector3.up) != 1)
            {
                return;
            }
            else
            {
                //continue
            }
        }
        
        //cache the place position and normal(rotation)
        Vector3 placePoint = hitInfo.point;
        Vector3 placePointNormal = hitInfo.normal;

        //set the held object to be null
        playerData.HandedSetObjectInHand(null, grabHand);

        //place the object
        ThisObject.transform.position = placePoint - Origin.localPosition;
        ThisObject.transform.up = placePointNormal;
        
        //unparent
        ThisObject.transform.parent = null;
        
        //reset scale
        ThisObject.transform.localScale = OldLocalScale;
        
        //reset oldScale
        OldLocalScale = Vector3.zero;
        
        //set all child objects and this object to be on old layer
        Utils.SetLayerRecursively(ThisObject, OldLayer);

        //make item look at player
        SetConstraintsActive(true);

        //call placed on the object 
        Placed(hitInfo.normal);
    }

    /// <summary>
    /// deletes the object
    /// </summary>
    /// <param name="grabHand">which hand this object is in</param>
    public void Consume(GrabHand grabHand)
    {
        //put the object down first so that the code doesnt freak out with missing reference errors
        PutDown(Vector3.zero,Vector3.zero, grabHand);
        //delete object
        GameObject.Destroy(ThisObject);
        //tell the object it was consumed
        Consumed();
    }

    /// <summary>
    /// called when the object was consumed
    /// </summary>
    public void Consumed()
    {
        
    }
    
    /// <summary>
    /// called when the object was grabbed
    /// </summary>
    public void Grabbed()
    {
        
    }

    /// <summary>
    /// called when the object is placed
    /// </summary>
    /// <param name="normal">the normal of the placed position</param>
    public void Placed(Vector3 normal)
    {
        
    }

    /// <summary>
    /// set if the object is looking at the main camera
    /// </summary>
    /// <param name="isActive">whether the object should look at the main camera or not</param>
    private void SetConstraintsActive(bool isActive)
    {
        //get the all AimConstraints on the object
        AimConstraint[] aimConstraints = ThisObject.GetComponentsInChildren<AimConstraint>();
        foreach (AimConstraint aimConstraint in aimConstraints)
        {
            //turn them on or off depending on isActive
            aimConstraint.constraintActive = isActive;
        }

    }
}