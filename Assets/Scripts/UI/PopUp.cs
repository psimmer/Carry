using System;
using UnityEngine;
using UnityEngine.UI;


public class PopUp : MonoBehaviour
{
    [Tooltip("How long the Popup will be active")]
    [SerializeField] float duration;
    [SerializeField] int timeOutDamagePatient;
    [SerializeField] float timeOutDamagePlayer;
    [SerializeField] Image radialBarImage;
    public Image RadialBarImage => radialBarImage;

    [SerializeField] Gradient gradient;
    [SerializeField] ItemSO item;
    float remainingHealingTimer;
    bool startedHealing;
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

    /// <summary>
    /// if the Radial bar of a popUp is zero or is full happens this.
    /// </summary>
    private void PopUpCondition()
    {
        if (radialBarImage.fillAmount <= 0)
        {
            if (GetComponentInParent<Patient>() != null)
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
            else
            {
                Destroy(this.gameObject);
                e_OnPopUpTimeOut?.Invoke(timeOutDamagePlayer);

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
    /// <summary>
    /// Let the radialBar go down
    /// </summary>
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
