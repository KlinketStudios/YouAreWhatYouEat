using UnityEngine;

public class HandAnimation : MonoBehaviour
{

    [SerializeField] private GameObject leftHand;
    [SerializeField] private GameObject rightHand;

    [SerializeField] private float verticalAmplitude;
    [SerializeField] private float verticalFrequency;
    [SerializeField] private float horizontalAmplitude;
    [SerializeField] private float horizontalFrequency;
    [SerializeField] private float horizontalSpeed;
    [SerializeField] private float verticalSpeed;

    [SerializeField] private PlayerController movementScript;

    private Vector3 leftHandStartPosition;
    private Vector3 rightHandStartPosition;

    private float offsetVertical;
    private float offsetHorizontal;

    private float timeLeft;
    private float timeRight = -.5f;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        leftHandStartPosition = leftHand.transform.localPosition;
        rightHandStartPosition = rightHand.transform.localPosition;
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
            
            leftHand.transform.localPosition = leftHandStartPosition + new Vector3(offsetHorizontal, offsetVertical, 0);
            rightHand.transform.localPosition = rightHandStartPosition + new Vector3(offsetHorizontal, offsetVertical, 0);
        }
    }
}
