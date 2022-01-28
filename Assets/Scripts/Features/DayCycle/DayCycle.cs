using System;
using UnityEngine;

public class DayCycle : MonoBehaviour
{
    [SerializeField] private float interpolationValue;
    private float interpolation;
    private Quaternion startPos;
    private Quaternion endPos;
    public Action dayCycle;


    private void Start()
    {
        startPos = Quaternion.Euler(10, -50, 0);
        endPos = Quaternion.Euler(180, -50, 0);
        dayCycle = LerpTheSun;
    }

    public void LerpTheSun()
    {
        interpolation += Time.deltaTime * (interpolationValue / 1000); 
        transform.rotation = Quaternion.Lerp(startPos, endPos, interpolation);
    }



}
