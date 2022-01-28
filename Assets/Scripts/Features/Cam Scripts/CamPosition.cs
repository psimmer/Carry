using UnityEngine;

public class CamPosition : MonoBehaviour
{
    [SerializeField] private Transform camPoints;
    public Transform currentPoint { get; set; }

    private float interpolation;

    private void Start()
    {
        currentPoint = camPoints.Find("Start");

    }

    private void Update()  
    {
        interpolation = Time.deltaTime * 0.5f;
        transform.position = Vector3.Lerp(transform.position, currentPoint.position, interpolation);
    }

   



}
