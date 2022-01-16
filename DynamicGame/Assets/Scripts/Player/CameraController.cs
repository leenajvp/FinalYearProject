using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float mSensitivity = 2;
    [SerializeField] 
    private float maxDown = -60F;
    [SerializeField] 
    private float maxUp = 60F;
    [SerializeField]
    float maxLeft = 60f;
    [SerializeField]
    float maxRight = 60f;


    private float rotationY = 0;
    private float rotationX = 0;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        CameraRotation();
    }

    void CameraRotation()
    {
        rotationX += Input.GetAxis("Mouse X") * mSensitivity;
        rotationX = Mathf.Clamp(rotationX, maxLeft, maxRight);

        rotationY += Input.GetAxis("Mouse Y") * mSensitivity;
        rotationY = Mathf.Clamp(rotationY, maxDown, maxUp);

        transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
    }
}

