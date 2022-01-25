using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPU : MonoBehaviour
{

    public string KeyTabs()
    {
        if (Input.anyKeyDown)
        {
            Debug.Log(Input.inputString);
            return Input.inputString;

        }
        else
            return null;
    }
}
