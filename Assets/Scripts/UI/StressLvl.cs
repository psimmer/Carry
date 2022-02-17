using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StressLvl : MonoBehaviour
{
    [SerializeField] private Image fillofStress;
    public Image FillOfStress { get { return fillofStress; } set { fillofStress = value; } }
    float fillAmountofStress;


    private void Awake()
    {
  
        fillAmountofStress = Random.Range(0,30);
        Debug.Log(fillAmountofStress);
        fillofStress.fillAmount = fillAmountofStress / 100;
    }

    public void SetStressLvl(Player player)
    {
        fillofStress.fillAmount = player.CurrentStressLvl;
    }


}
