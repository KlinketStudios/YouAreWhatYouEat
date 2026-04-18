using UnityEngine;

public class HandAnimation : MonoBehaviour
{

    [SerializeField] private SpriteRenderer leftHand;
    [SerializeField] private SpriteRenderer rightHand;
    [SerializeField] private GameObject handsParent;
    

    [SerializeField] private float verticalAmplitude;
    [SerializeField] private float verticalFrequency;
    [SerializeField] private float horizontalAmplitude;
    [SerializeField] private float horizontalFrequency;
    [SerializeField] private float horizontalSpeed;
    [SerializeField] private float verticalSpeed;

    [SerializeField] private PlayerController movementScript;

    [Header("Smooth Damp Variables")] 
    [SerializeField] private float smoothTime = .1f;
    [SerializeField] private float rotationSmoothTime = .6f;
    [SerializeField] private GameObject handFollowObject;
    [SerializeField] private Vector3 velocity = Vector3.zero;
    [SerializeField] private Vector3 rotationalVelocity = Vector3.zero;

    private Vector3 handFollowObjStartPos;


    private float offsetVertical;
    private float offsetHorizontal;

    private float timeLeft;
    private float timeRight = -.5f;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        handFollowObjStartPos = handFollowObject.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (movementScript.isMoving)
        {
            timeLeft += Time.deltaTime;
            timeRight += Time.deltaTime;
            
            offsetHorizontal += Mathf.Sin(timeLeft * horizontalFrequency) * horizontalAmplitude;
            offsetVertical += Mathf.Sin(timeRight * verticalFrequency) * verticalAmplitude;
            
            handFollowObject.transform.localPosition = handFollowObjStartPos + new Vector3(offsetHorizontal, offsetVertical, 0);
        }
        else
        {
            timeLeft += Time.deltaTime;
            timeRight += Time.deltaTime;
            
            offsetHorizontal += Mathf.Sin(timeLeft * (horizontalFrequency / 2)) * horizontalAmplitude;
            offsetVertical += Mathf.Sin(timeRight * (verticalFrequency / 2)) * verticalAmplitude;
            
            handFollowObject.transform.localPosition = handFollowObjStartPos + new Vector3(offsetHorizontal, offsetVertical, 0);

        }

        /*handsParent.transform.position = Vector3.SmoothDamp(handsParent.transform.position, handFollowObject.transform.position,
            ref velocity, smoothTime * Time.deltaTime);*/
        handsParent.transform.position = handFollowObject.transform.position;
        handsParent.transform.rotation = Quaternion.Slerp(handsParent.transform.rotation, handFollowObject.transform.rotation, rotationSmoothTime * Time.deltaTime);
    }
}
