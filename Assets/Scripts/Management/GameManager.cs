using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Patient patient;
    public event Action<Patient, Item> e_OnHeal;
    public event Action<Patient, Item> e_OnDamage;
    public Func<Patient> getPatient;

    private void Awake()
    {
        getPatient = patient.returnsHimself;
        e_OnHeal += player.HealPatientWithItem;
        e_OnDamage += player.DamagePatientWithItem;
    }

    void Update()
    {
        Treatment();
    }

    //I think this will work fine ^^
    public void Treatment()
    {
        if(patient.currentIllness == TaskType.ChangeBandage && player.currentItem.name == "Bandage")
        {
            e_OnHeal?.Invoke(getPatient(), player.currentItem);
        }
        else if (patient.currentIllness == TaskType.BringPills && player.currentItem.name == "Pill")
        {
            e_OnHeal?.Invoke(getPatient(), player.currentItem);
        }
        else
        {
            e_OnDamage(getPatient(), player.currentItem);
        }
    }



}
