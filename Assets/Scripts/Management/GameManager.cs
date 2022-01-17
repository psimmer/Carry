using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private List<Patient> patients;
    public event Action<Patient, Item> e_OnHeal;
    public event Action<Patient, Item> e_OnDamage;
    public Func<Patient> getPatient;

    //Just for now can be deleted later
    private bool touchPatient = false; // this bool stays false forever in moment

    private void Awake()
    {
        getPatient = patients[0].returnsHimself;     //we need to fix this index atomatically to the correct patient

        e_OnHeal += player.HealPatientWithItem;
        e_OnDamage += player.DamagePatientWithItem;
    }

    void Update()
    {
        Treatment(getPatient());
    }

    //I think this will work fine ^^
    public void Treatment(Patient patient)
    {
        if(player.currentItem != null && touchPatient)
        {
            if(patient.currentIllness == TaskType.ChangeBandage && player.currentItem.name == "Bandage")
            {
                e_OnHeal?.Invoke(patient, player.currentItem);
            }
            else if (patient.currentIllness == TaskType.BringPills && player.currentItem.name == "Pill")
            {
                e_OnHeal?.Invoke(patient, player.currentItem);
            }
            else
            {
                e_OnDamage(patient, player.currentItem);
            }

        }
    }



}
