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
    float joggingSpeed = 8f;
    [SerializeField]
    float runningSpeed = 11f;
    [SerializeField]
    float rotationSpeed = 10f;

    float lastReation = 0f;

    List<GameObject> boidsDetected = new List<GameObject>();
    public bool isDead = false;

    public bool isJogging { get; set; }
    public bool isRunning { get; set; }
    public bool isStopped { get; set; }


    public bool isDisabled = false;
    private KeyboardControls playerControls;

    private Animator animState;

    void Start()
    {
        animState = GetComponent<Animator>();
        playerControls = GetComponent<KeyboardControls>();
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

        if (isRunning)
        {
            speed = runningSpeed;
        }

        if (isJogging)
        {
            speed = joggingSpeed;
        }

        else if (!isRunning && !isJogging && !isStopped)
        {
            speed = walkingSpeed;
        }
    }

    public void Forward()
    {
        if (!isStopped)
            transform.position += transform.forward * speed * Time.deltaTime;

    }

    public void Backward()
    {
        if (!isStopped)
            transform.position -= transform.forward * speed * Time.deltaTime;

    }

    public void Jog()
    {

    }

    public void Run()
    {

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

    public void Swat()
    {
        if (Time.time < lastReation + 5.0f)
            return;

        lastReation = Time.time;

        speed = walkingSpeed;
    }



    public void Magic()
    {
        //if (!isDisabled)
        //{
        //    Collider[] colliders = Physics.OverlapSphere(transform.position, 15);

        //    foreach (var col in colliders)
        //    {
        //        BoidBehaviour followBoids = col.gameObject.GetComponent<BoidBehaviour>();

        //        if (followBoids != null)
        //        {
        //            boidsDetected.Add(col.gameObject);

        //            foreach (GameObject boid in boidsDetected)
        //            {
        //                followBoids.boidHealth = 0;
        //            }
        //        }
        //    }
        //}

    }

    public void BasicAttack()
    {
        //Debug.Log("Swing");
    }

    public void Kick()
    {
        //Debug.Log("Kick");
    }

    public void StrongAttack()
    {
        //Debug.Log("BigHit");
    }
}
