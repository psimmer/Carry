using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePoints : MonoBehaviour
{

    private void Update()
    {
        MoveCamera();
    }

    public void MoveCamera()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            transform.position += new Vector3(0, 0.01f, 0);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            transform.position -= new Vector3(0, 0.01f, 0);
        }
    }
}
