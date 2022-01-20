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
        interpolation = Time.deltaTime * 0.1f;

    }

    private void Update()  
    {
        transform.position = Vector3.Lerp(transform.position, currentPoint.position, interpolation);
    }

   



}
