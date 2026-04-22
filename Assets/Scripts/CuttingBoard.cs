using System;
using UnityEngine;

public class CuttingBoard : MonoBehaviour, IInteractable, IClickListener
{
    private IClickListener clickListener;

    private GameObject objectCutting;
    [SerializeField] private GameObject cutPoint;
    [SerializeField] private float stackDist;
    
    private CuttableIngredients[] spawnPointsInUse = new CuttableIngredients[] { (CuttableIngredients)(-1),(CuttableIngredients)(-1),(CuttableIngredients)(-1),(CuttableIngredients)(-1),(CuttableIngredients)(-1)};
    [SerializeField] private GameObject[] spawnPoints;
    private int[] spawnPointItemCount = new []{0,0,0,0,0};

    private PlayerData playerData;


    public void Interacted(GrabHand grabHand)
    {
        if (objectCutting != null)
        {
            GrabObject(objectCutting, grabHand);
        }
    }

    private void Start()
    {
        playerData = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>();
    }
    
    public void InteractedWithObjectInHand(GameObject obj, GrabHand grabHand)
    {
        if (obj.CompareTag("Knife") && objectCutting != null)
        {
            ICuttable iCuttable = objectCutting.GetComponent<ICuttable>();

            if ((iCuttable.CurrentCut += 1) <= iCuttable.CutAmount)
            {
                (GameObject,int) spawnPoint = FindCorrectSpawnPoint((CuttableIngredients)iCuttable.Product.GetComponent<InteractableIngredient>().type);

                Instantiate(iCuttable.Product, new Vector3(spawnPoint.Item1.transform.position.x,
                    spawnPoint.Item1.transform.position.y + stackDist * spawnPointItemCount[spawnPoint.Item2],
                        spawnPoint.Item1.transform.position.z) - iCuttable.Product.GetComponent<IIngredient>().Origin.transform.position, Quaternion.identity);

                if (iCuttable.CurrentCut == iCuttable.CutAmount)
                {
                    Destroy(objectCutting);
                }
            }

            return;
        }

        if (objectCutting == null && Enum.IsDefined(typeof(CuttableIngredients), obj.GetComponent<CuttableIngredient>().CuttableType))
        {
            PutObject(obj, grabHand);
        }
    }

    private void GrabObject(GameObject obj, GrabHand grabHand)
    {
        CuttableIngredient ingredient = obj.GetComponent<CuttableIngredient>();
        
        obj.GetComponent<IPickupAndPlaceable>().PickUp(grabHand);
        ingredient.ClickListener = null;
        objectCutting = null;

    }
    
    private void PutObject(GameObject obj, GrabHand grabHand)
    {
        CuttableIngredient ingredient = obj.GetComponent<CuttableIngredient>();
     
        objectCutting = obj;
        obj.GetComponent<IPickupAndPlaceable>().PutDown(cutPoint.transform.position - ingredient.Origin.transform.localPosition, Vector3.up, grabHand);
        ingredient.ClickListener = this;
    }

    private (GameObject,int) FindCorrectSpawnPoint(CuttableIngredients type)
    {
        int x = 0;
        foreach (var spawnPointType in spawnPointsInUse)
        {
            if (spawnPointType == type)
            {
                spawnPointItemCount[x]++;
                return (spawnPoints[x],x);
            }
            x++;
        }
        int i = 0;
        foreach (var spawnPointType in spawnPointsInUse)
        {
            if (spawnPointType == (CuttableIngredients)(-1))
            {
                spawnPointsInUse[i] = type;
                spawnPointItemCount[i]++;
                return (spawnPoints[i],i);
            }
            i++;
        }

        return (null, -1);
    }

    public void Click(GrabHand grabHand)
    {
        Interacted(grabHand);
    }

    public void ClickWithObjectInHand(GameObject obj, GrabHand grabHand)
    {
        InteractedWithObjectInHand(obj, grabHand);
    }
    
    public IClickListener ClickListener
    {
        get => clickListener;
        set => clickListener = value;
    }
}
