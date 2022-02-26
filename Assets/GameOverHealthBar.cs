using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverHealthBar : MonoBehaviour
{
    [SerializeField] Image heartBeat;
    float heartBeatTValue = 0;
    float heartbeatLerpSpeed = 0.4f;

    void Update()
    {
        if (heartBeat != null && heartBeat.IsActive())
        {
            heartBeatTValue += heartbeatLerpSpeed * Time.deltaTime;
            heartBeat.fillAmount = Mathf.Lerp(0, 1, heartBeatTValue);
            if (heartBeat.fillAmount == 1)
            {
                heartBeatTValue = 0;
                heartBeat.fillAmount = 0;
            }
        }
    }
}
