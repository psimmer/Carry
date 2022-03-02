using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverHints : MonoBehaviour
{
    [SerializeField] List<string> hints;
    [SerializeField] TextMeshProUGUI UIText;

    private void Start()
    {
        UIText.text += hints[Random.Range(0, hints.Count)];
    }


}
