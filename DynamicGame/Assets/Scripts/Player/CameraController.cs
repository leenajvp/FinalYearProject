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

    [SerializeField]
    private Transform player;
    [SerializeField]
    private float distance;
    [SerializeField]
    private float height;
    [SerializeField]
    private float damping;

    public Transform target;

    public bool smoothRotation = true;
    public bool followBehind = true;
    public float rotationDamping = 10.0f;
    public float speed;

    private Vector3 offset;

    private float rotationY = 0;
    private float rotationX = 0;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        //offset = transform.position - player.position;
        offset = target.transform.position - transform.position;
    }

    private void Update()
    {

        CameraPosition();
        CameraRotation();
    }

    void CameraRotation()
    {
        rotationX += Input.GetAxis("Mouse X") * mSensitivity;
       // rotationX = Mathf.Clamp(rotationX, maxLeft, maxRight);

        rotationY += Input.GetAxis("Mouse Y") * mSensitivity;
       // rotationY = Mathf.Clamp(rotationY, maxDown, maxUp);

        transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);

        //transform.position = player.position;

        //var newRotation = Quaternion.LookRotation(target.transform.position - transform.position);
        //transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, 1 * Time.deltaTime);

        //// Move
        // Vector3 newPosition = target.transform.position - target.transform.forward * offset.z - target.transform.up * offset.y;
        //  transform.position = Vector3.Slerp(transform.position, newPosition, Time.deltaTime * 1);

        //Vector3 newPosition = target.transform.position - target.transform.right * offset.x - target.transform.up * offset.y;
    }

    void CameraPosition()
    {
        var newRotation = Quaternion.LookRotation(target.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, speed * Time.deltaTime);

        Vector3 newPosition = target.transform.position - target.transform.forward * offset.z - target.transform.up * offset.y;
        transform.position = Vector3.Slerp(transform.position, newPosition, Time.deltaTime * 1);
    }
}

