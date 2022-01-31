using UnityEngine;

public class MovePoints : MonoBehaviour
{
    [SerializeField] private float cameraHeight;
    public float CamHeight => cameraHeight;

    [SerializeField] private float cameraDirection;
    public float CameraDirection => cameraDirection;

    [SerializeField] private float maxZoomOut;
    public float MaxZoomOut => maxZoomOut;

    [SerializeField] private Transform overViewPoint;
    public Transform OverViewPoint => overViewPoint;
    //[SerializeField] float interpolationValue;
    //bool IsQPressed;

    private void Update()
    {
        MoveCamera();

        //if (IsQPressed)
        //{
        //    float interpolation = Time.deltaTime * interpolationValue;
        //    Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, overViewPoint.position, interpolation);
        //}



    }

    public void MoveCamera()
    {

        if (Input.GetKey(KeyCode.Q))
        {
            //IsQPressed = true;
            transform.position += new Vector3(0, cameraHeight, -cameraDirection) * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.E))
            transform.position -= new Vector3(0, cameraHeight, -cameraDirection) * Time.deltaTime;

        transform.position = Vector3.ClampMagnitude(transform.position, MaxZoomOut);
        //transform.position = Vector3.ClampMagnitude(transform.position, MaxZoomIn);

        //if (Input.GetKeyUp(KeyCode.Q))
        //{
        //    //IsQPressed = false;
        //}

    }
}
