using UnityEngine;

public class CamPosition : MonoBehaviour
{
    [SerializeField] private Transform camPoints;
    [SerializeField] MovePoints movePoints;
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

        if (Input.GetKey(KeyCode.Q) && transform.position.y < 18.78f)
        {
            transform.position += new Vector3(0, movePoints.CamSpeed, -movePoints.CamSpeed);
        }
        else if (Input.GetKey(KeyCode.E) && transform.position.y > 11.7f)
        {
            transform.position -= new Vector3(0, movePoints.CamSpeed, -movePoints.CamSpeed);
        }

        //Vector3.ClampMagnitude(transform.position, movePoints.MaxZoomOut);
    }





}
