using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] private Slider healthbar;
    private float maxAmount;
    private float minAmount;
    private float currentAmount;


    private void Start()
    {
        maxAmount = 100;
        minAmount = 0;
    }





}
