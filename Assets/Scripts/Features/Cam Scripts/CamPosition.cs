using UnityEngine;

public class CamPosition : MonoBehaviour
{
    [SerializeField] private Transform camPoints;
    [SerializeField] MovePoints movePoints;
    [SerializeField] float cameraSpeedOut;
    [SerializeField] float cameraSpeedIn;

    [SerializeField] public Transform currentPoint;
    [SerializeField] public Transform lastPoint;

    private float interpolation;

    private void Update()
    {
        if (!movePoints.IsCameraFixed)
        {
            interpolation = Time.deltaTime * cameraSpeedIn;
            transform.position = Vector3.Lerp(transform.position, lastPoint.position, interpolation);
        }
        else
        {
            interpolation = Time.deltaTime * cameraSpeedOut;
            transform.position = Vector3.Lerp(transform.position, movePoints.OverViewPoint.position, interpolation);
        }

        if (Input.GetKey(KeyCode.Q) && transform.position.y < 15.249f)
        {
            currentPoint = movePoints.OverViewPoint;
            transform.position += new Vector3(0, movePoints.CamHeight, -movePoints.CameraDirection) * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.E) && transform.position.y > 10.174f)
        {
            //currentPoint = lastPoint;
            transform.position -= new Vector3(0, movePoints.CamHeight, -movePoints.CameraDirection) * Time.deltaTime;
        }
    }





}
