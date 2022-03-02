using UnityEngine;

public class CamColliders : MonoBehaviour
{

    //EveryRoom haves Colliders and evry room haves a own CameraPosition

    [SerializeField] private Transform newPosition;

    public Transform NewPosition => newPosition;


}
