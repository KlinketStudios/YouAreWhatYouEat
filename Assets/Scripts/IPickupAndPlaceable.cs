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

    public void PickUp(GrabHand grabHand)
    {
        
        var playerData = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>();
        if (playerData.HandedIsHolding(grabHand))
            return;

        playerData.HandedSetObjectInHand(ThisObject, grabHand);

        var grabPoint = playerData.HandedGrabPoint(grabHand);

        ThisObject.transform.position = grabPoint.position;
        ThisObject.transform.parent = grabPoint;
        OldLayer = ThisObject.layer;
        
        OldLocalScale = ThisObject.transform.lossyScale;
        ThisObject.transform.localScale = grabPoint.localScale;
        
        Utils.SetLayerRecursively(ThisObject, LayerMask.NameToLayer(playerData.grabLayer));

        SetConstraintsActive(false);
        
        Grabbed();
    }

    public void PutDown(Vector3 placePoint, Vector3 placePointNormal, GrabHand grabHand)
    {
        var playerData = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>();

        playerData.HandedSetObjectInHand(null, grabHand);

        ThisObject.transform.position = placePoint - Origin.localPosition;
        ThisObject.transform.up = placePointNormal;
        ThisObject.transform.parent = null;
        
        ThisObject.transform.localScale = OldLocalScale;
        OldLocalScale = Vector3.zero;
        
        Utils.SetLayerRecursively(ThisObject, OldLayer);

        SetConstraintsActive(true);
        
        Placed(placePointNormal);
    }
    public void PutDownAtLookPoint(GrabHand grabHand)
    {
        var playerData = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>();
        var playerInteract = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInteract>();
        
        var didHitObject = Physics.Raycast(playerInteract.cameraTransform.position, playerInteract.cameraTransform.forward,
            out var hitInfo, playerInteract.interactDist, playerInteract.interactLayerMask);

        if (PlaceableOnWalls)
        {
            if (Vector3.Dot(hitInfo.normal, Vector3.up) != 1)
            {
                
                if (Mathf.Abs(Vector3.Dot(hitInfo.normal, Vector3.up)) <= 0.25f)
                {
                }
                else
                {
                    return;
                }
            }
            else
            {
            }
        }
        else
        {
            if (Vector3.Dot(hitInfo.normal, Vector3.up) != 1)
            {
                
                if (Mathf.Abs(Vector3.Dot(hitInfo.normal, Vector3.up)) <= 0.25f)
                {
                    return;
                }
                else
                {
                    return;
                }
            }
            else
            {
            }
        }
        
        Vector3 placePoint = hitInfo.point;
        Vector3 placePointNormal = hitInfo.normal;

        playerData.HandedSetObjectInHand(null, grabHand);

        ThisObject.transform.position = placePoint - Origin.localPosition;
        ThisObject.transform.up = placePointNormal;
        ThisObject.transform.parent = null;
        
        ThisObject.transform.localScale = OldLocalScale;
        OldLocalScale = Vector3.zero;
        
        Utils.SetLayerRecursively(ThisObject, OldLayer);

        SetConstraintsActive(true);

        Placed(hitInfo.normal);
    }

    public void Consume(GrabHand grabHand)
    {
        PutDown(Vector3.zero,Vector3.zero, grabHand);
        GameObject.Destroy(ThisObject);
    }
    
    public void Grabbed()
    {
        
    }

    public void Placed(Vector3 normal)
    {
        
    }

    private void SetConstraintsActive(bool isActive)
    {
        AimConstraint[] aimConstraints = ThisObject.GetComponentsInChildren<AimConstraint>();
        foreach (AimConstraint aimConstraint in aimConstraints)
        {
            aimConstraint.constraintActive = isActive;
        }

    }
}