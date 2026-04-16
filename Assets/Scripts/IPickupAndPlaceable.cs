using KlinketStudiosTools.Utils;
using UnityEngine;

public interface IPickupAndPlaceable
{
    public Transform Origin { get; set; }
    public GameObject ThisObject { get; set; }
    public int OldLayer { get; set; }

    public void PickUp(GrabHand grabHand)
    {
        var playerData = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>();

        playerData.HandedSetObjectInHand(ThisObject, grabHand);

        var grabPoint = playerData.HandedGrabPoint(grabHand);

        ThisObject.transform.position = grabPoint.position;
        ThisObject.transform.parent = grabPoint;
        OldLayer = ThisObject.layer;
        Utils.SetLayerRecursively(ThisObject, LayerMask.NameToLayer(playerData.grabLayer));

        Grabbed();
    }

    public void PutDown(Vector3 placePoint, Vector3 placePointNormal, GrabHand grabHand)
    {
        var playerData = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>();

        playerData.HandedSetObjectInHand(null, grabHand);

        ThisObject.transform.position = placePoint - Origin.localPosition;
        ThisObject.transform.up = placePointNormal;
        ThisObject.transform.parent = null;
        Utils.SetLayerRecursively(ThisObject, OldLayer);

        Placed();
    }

    public void Consume(GrabHand grabHand)
    {
        PutDown(Vector3.zero,Vector3.zero, grabHand);
        GameObject.Destroy(ThisObject);
    }
    
    public void Grabbed();
    public void Placed();
}