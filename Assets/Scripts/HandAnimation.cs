using System;
using UnityEngine;

public class HandAnimation : MonoBehaviour
{

    [SerializeField] private GameObject handsParent;
    private PlayerController movementScript;
    private PlayerData playerData;
    
    [Header("Wave Variables"),SerializeField] private float verticalAmplitude;
    [SerializeField] private float verticalFrequency;
    [SerializeField] private float horizontalAmplitude;
    [SerializeField] private float horizontalFrequency;


    [Header("Smooth Damp Variables")] 
    [SerializeField] private GameObject handFollowObject;
    [SerializeField] private float rotationSmoothTime = 50;
    [SerializeField] private float maxAngle = 10;
    private float currentAngle;
    
    private Vector3 handFollowObjStartPos;
    private float time;

    [Header("Hand State")] [SerializeField]
    private Sprite handFullSprite;
    [SerializeField] private Sprite handEmptySprite;
    [SerializeField] private Sprite handHoldingKnifeSprite;

    [SerializeField] private SpriteRenderer leftHandSpriteRenderer, 
        rightHandSpriteRenderer;
    
    private HandState leftHandState;
    private HandState rightHandState;
    private bool isLeftHandDirty;
    private bool isRightHandDirty;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        handFollowObjStartPos = handFollowObject.transform.localPosition;
        movementScript = GetComponent<PlayerController>();
        playerData = GetComponent<PlayerData>();
    }

    // Update is called once per frame
    void Update()
    {
        SmoothDampToLookRotation();

        CheckHandState();
        
        CheckLeftHandSprite();
        CheckRightHandSprite();
    }

    private void CheckLeftHandSprite()
    {
        if (isLeftHandDirty)
        {
            switch (leftHandState)
            {
                case HandState.Empty:
                    leftHandSpriteRenderer.sprite = handEmptySprite;
                    break;
                case HandState.Full:
                    leftHandSpriteRenderer.sprite = handFullSprite;
                    break;
                case HandState.HoldingKnife:
                    leftHandSpriteRenderer.sprite = handHoldingKnifeSprite;
                    break;
            }
            isLeftHandDirty = false;
        }
    }
    
    private void CheckRightHandSprite()
    {
        if (isRightHandDirty)
        {
            switch (rightHandState)
            {
                case HandState.Empty:
                    rightHandSpriteRenderer.sprite = handEmptySprite;
                    break;
                case HandState.Full:
                    rightHandSpriteRenderer.sprite = handFullSprite;
                    break;
                case HandState.HoldingKnife:
                    rightHandSpriteRenderer.sprite = handHoldingKnifeSprite;
                    break;
            }
            isRightHandDirty = false;
        }
    }

    private void CheckHandState()
    {
        //Left Hand
        if (playerData.HandedIsHolding(GrabHand.LeftHand))
            if (playerData.HandedHeldObject(GrabHand.LeftHand).CompareTag("Knife"))
            {
                leftHandState = HandState.HoldingKnife;
                isLeftHandDirty = true;
            }
            else
            {
                leftHandState = HandState.Full;
                isLeftHandDirty = true;
            }
        else
        {
            leftHandState = HandState.Empty;
            isLeftHandDirty = true;
        }
        
        //Right Hand
        if (playerData.HandedIsHolding(GrabHand.RightHand))
            if (playerData.HandedHeldObject(GrabHand.RightHand).CompareTag("Knife"))
            {
                rightHandState = HandState.HoldingKnife;
                isRightHandDirty = true;
            }
            else
            {
                rightHandState = HandState.Full;
                isRightHandDirty = true;
            }
        else
        {
            rightHandState = HandState.Empty;
            isRightHandDirty = true;
        }
    }
    
    private void SmoothDampToLookRotation()
    {
        if (movementScript.isMoving)
        {
            time += Time.deltaTime;
        }
        else
        {
            time += Time.deltaTime / 2;
        }

        float offsetHorizontal = handFollowObjStartPos.x + Mathf.Sin(time * horizontalFrequency) * horizontalAmplitude;
        float offsetVertical = handFollowObjStartPos.y + Mathf.Sin(time * verticalFrequency) * verticalAmplitude;
            
        handFollowObject.transform.localPosition = new Vector3(offsetHorizontal, offsetVertical, handFollowObject.transform.localPosition.z);

        handsParent.transform.position = handFollowObject.transform.position;

        currentAngle = Quaternion.Angle(handsParent.transform.rotation, handFollowObject.transform.rotation);

        if (currentAngle > maxAngle)
        {
            handsParent.transform.rotation = Quaternion.RotateTowards(handFollowObject.transform.rotation,
                handsParent.transform.rotation, maxAngle);
        }
        else
        {
            handsParent.transform.rotation = Quaternion.Slerp(handsParent.transform.rotation, handFollowObject.transform.rotation, rotationSmoothTime * Time.deltaTime);
        }
    }

    private enum HandState
    {
        Empty,
        Full,
        HoldingKnife
    }
}
