using UnityEngine;

public class TrashCan : MonoBehaviour, IInteractable
{
    private IClickListener clickListener;

    public IClickListener ClickListener
    {
        get => clickListener;
        set => clickListener = value;
    }

    public void Interacted(GrabHand grabHand)
    {
    }

    public void InteractedWithObjectInHand(GameObject obj, GrabHand grabhand)
    {
        //check if held used item is trashable
        if (!obj.TryGetComponent(out ITrashable trashable))
            return;
        
        //delete obejct
        obj.GetComponent<IPickupAndPlaceable>().PutDown(Vector3.zero, Vector3.zero, grabhand);
        Destroy(obj);
    }
}