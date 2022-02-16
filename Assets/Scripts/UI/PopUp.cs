using System;
using UnityEngine;
using UnityEngine.UI;


public class PopUp : MonoBehaviour
{
    [SerializeField] float duration;
    [SerializeField] Image radialBarImage;
    public Image RadialBarImage => radialBarImage;

    [SerializeField] Gradient gradient;
    [SerializeField] int timeOutDamagePatient;
    [SerializeField] float timeOutDamagePlayer;
    [SerializeField] ItemSO item;
    float remainingHealingTimer;
    bool startedHealing;
    // [SerializeField] float healingSpeed; this variable we dont need
    public static event Action<float> e_OnPopUpTimeOut;
    public static event Action<Patient> e_RemovePatient;

    public TaskType popUpTaskType;
    public TaskType TaskType { get { return popUpTaskType; } set { popUpTaskType = value; } }

    private bool isHealing;
    public bool IsHealing { get { return isHealing; } set { isHealing = value; } }

    private float timePassed;


    private void Start()
    {
        timePassed = duration;
    }

    private void Update()
    {
        if (radialBarImage != null)
        {
            UpdateRadialBar();
            PopUpCondition();
            PopUpAnimation();
        }
    }

    private void PopUpCondition()
    {
        if (radialBarImage.fillAmount <= 0)
        {
            GetComponentInParent<Patient>().Treatment(-timeOutDamagePatient);
            e_OnPopUpTimeOut?.Invoke(timeOutDamagePlayer);
            GetComponentInParent<Patient>().HasPopUp = false;
            Destroy(this.gameObject);
            if (GetComponentInParent<Patient>().CurrentHP <= 0)
            {
                e_RemovePatient?.Invoke(GetComponentInParent<Patient>());
            }
        }
        else if (radialBarImage.fillAmount >= 1)
        {
            GetComponentInParent<Patient>().HasPopUp = false;
        }
    }

    private void UpdateRadialBar()
    {

        if (isHealing)
        {
            if (!startedHealing)  // this saves the time that has been running already in the variable remainingHealingTimer
            {
                remainingHealingTimer = duration - timePassed; // for example, if the total duration of an item is 20 seconds, and 4 seconds passed already, the variable will take the value 4
                startedHealing = true; // it only does in the first frame of the healing system, so the variable wnot change mid-healing
            }
            timePassed += remainingHealingTimer / item.healingTime * Time.deltaTime; // we divide the variable mentioned by the desired healing time, and multiply it by the delta time
        }
        else
            timePassed -= Time.deltaTime;

        radialBarImage.fillAmount = timePassed / duration;
        radialBarImage.color = gradient.Evaluate(radialBarImage.fillAmount);
    }

    private void PopUpAnimation()
    {
        if (radialBarImage.fillAmount >= 0.25)
        {
            GetComponentInChildren<Animator>().SetBool("doAnimation", false);
        }
        else if (radialBarImage.fillAmount < 0.25)
        {
            GetComponentInChildren<Animator>().SetBool("doAnimation", true);
        }
    }
}
