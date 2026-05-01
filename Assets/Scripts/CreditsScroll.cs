using System;
using UnityEngine;

public class CreditsScroll : MonoBehaviour
{
    private Vector3 startPosition;
    [SerializeField] private float speed;
    

    void OnEnable()
    {
        if (startPosition == Vector3.zero)
            startPosition = GetComponent<RectTransform>().localPosition;
        GetComponent<RectTransform>().localPosition = startPosition;
    }
    
    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0, speed * Time.deltaTime, 0);
    }
}
