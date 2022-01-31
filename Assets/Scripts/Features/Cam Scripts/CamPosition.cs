using UnityEngine;

public class CamPosition : MonoBehaviour
{
    [SerializeField] private Transform camPoints;
    [SerializeField] MovePoints movePoints;
    [SerializeField] float cameraSpeed;

    [SerializeField] public Transform currentPoint;

    private float interpolation;
    private void Update()
    {
        interpolation = Time.deltaTime * cameraSpeed;
        transform.position = Vector3.Lerp(transform.position, currentPoint.position, interpolation);

        if (Input.GetKey(KeyCode.Q) && transform.position.y < 20.325f)
        {
            currentPoint = movePoints.OverViewPoint;
            transform.position += new Vector3(0, movePoints.CamHeight, -movePoints.CameraDirection) * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.E) && transform.position.y > 10.174f)
        {
            transform.position -= new Vector3(0, movePoints.CamHeight, -movePoints.CameraDirection) * Time.deltaTime;
        }
    }





}
