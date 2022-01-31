using UnityEngine;

public class MovePoints : MonoBehaviour
{
    [SerializeField] private float cameraHeight;
    public float CamHeight => cameraHeight;

    [SerializeField] private float cameraDirection;
    public float CameraDirection => cameraDirection;

    [SerializeField] private float maxZoomOut;
    public float MaxZoomOut => maxZoomOut;


    private void Update()
    {
        MoveCamera();
    }

    public void MoveCamera()
    {

        if (Input.GetKey(KeyCode.Q))
            transform.position += new Vector3(0, cameraHeight, -cameraDirection) * Time.deltaTime;
        else if(Input.GetKey(KeyCode.E))
            transform.position -= new Vector3(0, cameraHeight, -cameraDirection) * Time.deltaTime;

        transform.position = Vector3.ClampMagnitude(transform.position, MaxZoomOut);
        //transform.position = Vector3.ClampMagnitude(transform.position, MaxZoomIn);
    }
}
