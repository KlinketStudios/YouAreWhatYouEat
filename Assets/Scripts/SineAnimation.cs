using System;
using UnityEngine;

public class SineAnimation : MonoBehaviour
{

    [SerializeField, Range(-20, 20)] private float amplitude;
    [SerializeField, Range(-20, 20)] private float frequency;
    private Vector3 startPosition;
    private float t;
    [SerializeField] private Axis axis;

    private enum Axis
    {
        UpDown,
        LeftRight,
        FrontBack
    }
    
    private void Start()
    {
        //cache start position
        startPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        //move this object according to all the axis
        t += Time.deltaTime;
        switch (axis)
        {
            case Axis.UpDown:
                transform.localPosition = new Vector3(transform.localPosition.x, startPosition.y + Mathf.Sin(t * frequency) * amplitude,transform.localPosition.z);
                break;
            case Axis.LeftRight:
                transform.localPosition = new Vector3(startPosition.x + Mathf.Sin(t * frequency) * amplitude, transform.localPosition.y,transform.localPosition.z);
                break;
            case Axis.FrontBack:
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, startPosition.z + Mathf.Sin(t * frequency) * amplitude);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

    }
}
