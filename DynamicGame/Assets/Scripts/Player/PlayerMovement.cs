using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour, IPlayerControls
{
    public float health = 10;
    public float speed = 2f;
    [SerializeField]
    float walkingSpeed = 4f;
    [SerializeField]
    float runSpeed = 8f;
    [SerializeField]
    float crawlSpeed = 11f;
    [SerializeField]
    float rotationSpeed = 10f;

    float lastReation = 0f;

    [SerializeField]
    GameObject sword;
    private Sword swordScript;
    [SerializeField]
    GameObject gun;
    private Gun gunScript;
    public int currentGun = 0;

    public bool isDead = false;
    public bool isCrawling { get; set; }
    public bool isRunning { get; set; }
    public bool isStopped { get; set; }


    public bool isDisabled = false;
    private KeyboardControls playerControls;

    private Animator animState;
    private static readonly int activeGun = Animator.StringToHash("ActiveGun");

    void Start()
    {
        animState = GetComponent<Animator>();
        playerControls = GetComponent<KeyboardControls>();

        swordScript = sword.GetComponent<Sword>();
        gunScript = gun.GetComponent<Gun>();

        currentGun = 0;
        animState.SetBool(activeGun, true);
    }

    void Update()
    {

        if (health <= 0)
        {
            isDead = true;
        }

        if (isStopped)
        {
            speed = 0f;
        }
        

        Shoot();

    }

    public void ManageSpeed()
    {
        if (isRunning)
        {
            speed = crawlSpeed;
        }

        else if (isCrawling)
        {
            speed = runSpeed;
        }

        else// (!isRunning && !isCrawling && !isStopped)
        {
            speed = walkingSpeed;
        }
    }

    public void Forward()
    {
        if (!isStopped)
        {
            transform.position += transform.forward * speed * Time.deltaTime;
            ManageSpeed();
        }
    }

    public void Backward()
    {
        if (!isStopped)
        {
            transform.position -= transform.forward * speed * Time.deltaTime;
            ManageSpeed();
        }
    }

    public void TurnLeft()
    {
        transform.Rotate(0.0f, -rotationSpeed * Time.deltaTime, 0.0f);
    }

    public void TurnRight()
    {
        transform.Rotate(0.0f, rotationSpeed * Time.deltaTime, 0.0f);
    }

    public void PickUp()
    {

    }

    public void ChangeGun()
    {
        if (currentGun == 1)
        {
            currentGun = 0;
            animState.SetBool(activeGun, true);
        }


        else 
        {
            currentGun = 1;
            animState.SetBool(activeGun, false);
        }
            
    }

    public void WeaponAttack()
    {
        if (currentGun == 1)
            SwordAttack();

        else
            Shoot();    
    }

    public void SwordAttack()
    {
        if(swordScript.isCollected && swordScript.inUse)
        Debug.Log("Sword");
    }

    private float rayDistance = 10.0f;
    public GameObject holdPos;
    [SerializeField]
    private float mSensitivity = 2;
    [SerializeField]
    private float maxDown = -60F;
    [SerializeField]
    private float maxUp = 60F;

    private float rotationY = 0;
    private RaycastHit hit;

    public void Shoot()
    {
        

        if (gunScript.inUse)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(holdPos.transform.position, holdPos.transform.forward * 100, Color.red);
            if (Physics.Raycast(ray, out hit, 50))
            {

                if(hit.collider != null)
                {
                    //transform.LookAt(hit.point);
                    if (hit.collider.gameObject.tag != "Untagged")
                    {

                        // transform.LookAt(Vector3.zero);
                        //transform.localEulerAngles = new Vector3(0, hit.transform.position, 0);
                        //transform.localEulerAngles = new Vector3(-rotationY, 0, 0);
                        Debug.Log("hit Enemy");
                        // float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mSensitivity;
                        // rotationY += Input.GetAxis("Mouse Y") * mSensitivity;
                        //  rotationY = Mathf.Clamp(rotationY, maxDown, maxUp);
                        //values that will be set in the Inspector
                        Transform Target = hit.transform;
                        float RotationSpeed = 1;

                        //values for internal use
                        Quaternion _lookRotation;
                        Vector3 _direction;

                        // Update is called once per frame

                        //find the vector pointing from our position to the target
                        _direction = (Target.position - transform.position).normalized;

                        //create the rotation we need to be in to look at the target
                        _lookRotation = Quaternion.LookRotation(_direction);

                        //rotate us over time according to speed until we are in the required rotation
                        transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * RotationSpeed);

                    }
                }
            }

        }
    }

    public void Kick()
    {
        Debug.Log("Kick");
    }

}
