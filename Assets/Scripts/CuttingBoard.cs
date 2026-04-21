using System;
using UnityEngine;

public class CuttingBoard : MonoBehaviour, IInteractable, IClickListener
{
    private IClickListener clickListener;

    private GameObject objectCutting;
    private GameObject cutPoint;

    private CuttableIngredients[] spawnPointsInUse = new CuttableIngredients[] { (CuttableIngredients)(-1),(CuttableIngredients)(-1),(CuttableIngredients)(-1),(CuttableIngredients)(-1),(CuttableIngredients)(-1)};
    private GameObject[] spawnPoints;

    private int currentCuts;
    private PlayerData playerData;

    public IClickListener ClickListener
    {
        get => clickListener;
        set => clickListener = value;
    }

    public void Interacted(GrabHand grabHand)
    {
        
    }

    private void Start()
    {
        playerData = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>();
    }

    public void InteractedWithObjectInHand(GameObject obj, GrabHand grabHand)
    {
        if (obj.CompareTag("Knife") && objectCutting != null)
        {
            ICuttable iCuttable = obj.GetComponent<ICuttable>();

            if ((currentCuts += 1) >= iCuttable.CutAmount)
            {
                iCuttable.CutAmount -= 1;
                GameObject spawnPoint = FindCorrectSpawnPoint((CuttableIngredients)iCuttable.Product.GetComponent<InteractableIngredient>().type);

                Instantiate(iCuttable.Product, spawnPoint.transform.position, Quaternion.identity).GetComponent<InteractableIngredient>();

                if (currentCuts == iCuttable.CutAmount)
                {
                    Destroy(objectCutting);
                }
            }

            return;
        }

        if (objectCutting == null && Enum.IsDefined(typeof(CuttableIngredients), (CuttableIngredients)obj.GetComponent<IIngredient>().Type))
        {
            InteractableIngredient ingredient = obj.GetComponent<InteractableIngredient>();
            
            objectCutting = obj;
            obj.GetComponent<IPickupAndPlaceable>().PutDown(cutPoint.transform.position - ingredient.Origin.transform.localPosition, Vector3.up, grabHand);
            ingredient.ClickListener = this;
        }
    }

    private GameObject FindCorrectSpawnPoint(CuttableIngredients type)
    {
        foreach (var spawnPointType in spawnPointsInUse)
        {
            if (spawnPointType == type)
            {
                return gameObject;
            }
        }
        foreach (var spawnPointType in spawnPointsInUse)
        {
            if (spawnPointType == (CuttableIngredients)(-1))
            {
                return gameObject;
            }
        }

        return null;
    }

    public void Click(GrabHand grabHand)
    {
        throw new NotImplementedException();
    }

    public void ClickWithObjectInHand(GameObject obj, GrabHand grabHand)
    {
        throw new NotImplementedException();
    }
}
