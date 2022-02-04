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
    bool isCameraFixed;
    public bool IsCameraFixed { get { return isCameraFixed; } set { isCameraFixed = value; } }

    private void Awake()
    {
        IsCameraFixed = true;
    }

    private void Update()
    {
        MoveCamera();


    }

    public void MoveCamera()
    {

        if (Input.GetKey(KeyCode.Q))
        {
            IsCameraFixed = true;
            transform.position += new Vector3(0, cameraHeight, -cameraDirection) * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            IsCameraFixed = false;
            transform.position -= new Vector3(0, cameraHeight, -cameraDirection) * Time.deltaTime;
        }

        transform.position = Vector3.ClampMagnitude(transform.position, MaxZoomOut);

    }
}
