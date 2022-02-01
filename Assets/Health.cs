using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private GameObject initialpatient;
    public float currentHp;
    public float currentScale;
    private Patient patient;
    private void Start()
    {
        patient = initialpatient.GetComponent<Patient>();
        currentHp = patient.CurrentHP;
        currentScale = transform.localScale.x;
    }
    private void Update()
    {
            currentScale = patient.CurrentHP / 100;
    }
}
