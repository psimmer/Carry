using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamPosition : MonoBehaviour
{
    [SerializeField] private Transform camPoints;
    public Transform currentPoint { get; set; }

    private float interpolation;

    private void Start()
    {
        currentPoint = camPoints.Find("Start");
    }

    private void Update()  
    {
        // Issue!!! We need to reset the interpolation to zero after a Lerp
        transform.position = Vector3.Lerp(transform.position, currentPoint.position, interpolation);
        interpolation += Time.deltaTime * 0.0005f;
        if (transform.position == currentPoint.position)
        {
            interpolation = 0; 
        }

    }





}
