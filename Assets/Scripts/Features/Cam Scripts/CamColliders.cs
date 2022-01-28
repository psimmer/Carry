using UnityEngine;

public class CamColliders : MonoBehaviour
{
    [SerializeField] private Transform firstPosition;
    [SerializeField] private Transform newPosition;

    public Transform FirstPosition => firstPosition;
    public Transform NewPosition => newPosition;


}
