using UnityEngine;

public class TrashCan : MonoBehaviour, IInteractable
{
    public void Interacted(GrabHand grabHand)
    {
        
    }

    public void InteractedWithObjectInHand(GameObject obj, GrabHand grabhand)
    {
        obj.GetComponent<IPickupAndPlaceable>().PutDown(Vector3.zero, Vector3.zero, grabhand);

        Destroy(obj);
    }
}
