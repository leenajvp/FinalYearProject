using Bullets;
using DDA;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(PlayerController), typeof(PlayerInput), typeof(CharacterController))]

    public class PlayerController : MonoBehaviour, IPlayer
    {
        [Header("Player movement values")]
        [SerializeField] private Vector3 playerVelocity;
        [SerializeField] private float playerSpeed = 2.0f;
        [SerializeField] private float rotSpeed = 5.0f;
        [SerializeField] private float jumpHeight = 1.0f;
        [SerializeField] private float pushForce = 1.0f;
        [SerializeField] private float gravityValue = -10f;
        [SerializeField] private bool grounded;
        private float currentSpeed;

        [Header("Shooting controls")]
        [Tooltip("Position to shoot bullet from")]
        [SerializeField] private Transform shootPoint;
        [Tooltip("Object pool for bullets")]
        [SerializeField] private ObjectPool bulletPool;
        [SerializeField] private ParticleSystem muzzleFlash;
        [SerializeField] private ParticleSystem impactEffect;

        [Header("Interactions")]
        public bool interacting = false;
        public bool availableInteraction = false;
        [SerializeField] private Camera[] cinemachineCams;

        private Transform cameraTransform;
        private CharacterController controller;
        private PlayerInput playerInput;
        private InputAction moveAction;
        private InputAction lookAction;
        private InputAction jumpAction;
        private InputAction shootAction;
        private InputAction interactAction;
        private InputAction dragAction;
        private Collider[] colliders;

        private DDAManager ddaManager;

        [SerializeField] private LayerMask playerMask;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            playerInput = GetComponent<PlayerInput>();
            moveAction = playerInput.actions["Move"];
            lookAction = playerInput.actions["Look"];
            jumpAction = playerInput.actions["Jump"];
            shootAction = playerInput.actions["Shoot"];
            interactAction = playerInput.actions["Interact"];
            dragAction = playerInput.actions["Drag"];

            if (ddaManager == null)
            {
                ddaManager = FindObjectOfType<DDAManager>();
            }
        }

        private void OnEnable()
        {
            shootAction.performed += _ => Shoot();
        }

        private void OnDisable()
        {
            shootAction.performed -= _ => Shoot();
        }

        private void Start()
        {
            cameraTransform = Camera.main.transform;
            currentSpeed = playerSpeed;
            colliders = Physics.OverlapSphere(transform.position, 3);
        }

        void Update()
        {
            GetInteraction();
           // PullObjects();
            Jump();
            Move();

            if (interacting)
            {
                moveAction.Disable();
                jumpAction.Disable();
                lookAction.Disable();
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }


            else
            {
                moveAction.Enable();
                jumpAction.Enable();
                lookAction.Enable();
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }

        }

        private void Move()
        {
            Vector2 input = moveAction.ReadValue<Vector2>();
            Vector3 move = new Vector3(input.x, 0, input.y);
            move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;
            move.y = 0f;

            controller.Move(move * Time.deltaTime * currentSpeed);
            playerVelocity.y += gravityValue * Time.deltaTime;
            controller.Move(playerVelocity * Time.deltaTime);

            //Rotate with camera
            Quaternion targetRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotSpeed * Time.deltaTime);
        }

        private void Jump()
        {
            grounded = controller.isGrounded;
            if (grounded && playerVelocity.y < 0)
            {
                playerVelocity.y = 0f;
            }

            // Changes the height position of the player.
            if (jumpAction.triggered && grounded)
            {
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            }
        }

        private void Shoot()
        {
            // ddaManager.ManageEnemyHealth();
            ddaManager.currentsShots++;

            muzzleFlash.Play();

            RaycastHit hit;
            GameObject newBullet = bulletPool.GetObject();
            newBullet.transform.position = shootPoint.position;
            newBullet.transform.rotation = shootPoint.rotation;
            newBullet.SetActive(true);
            BulletController bulletController = newBullet.GetComponent<BulletController>();

            if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, Mathf.Infinity))
            {
                bulletController.target = hit.point;
                bulletController.hit = true;
                var newImpact = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(newImpact, 0.6f);
            }

            else
            {
                bulletController.target = cameraTransform.position + cameraTransform.forward * 25;
                bulletController.hit = false;
            }
        }

        private void GetInteraction()
        {
            foreach (Collider col in colliders)
            {
                Iinteractive interactive = col.gameObject.GetComponent<Iinteractive>();

                if (availableInteraction && interactive != null)
                {
                    if (interactAction.triggered)
                    {
                        interactive.GetPuzzle();
                        moveAction.Disable();
                        shootAction.Disable();
                        jumpAction.Disable();
                    }

                }
            }
        }

        private void PullObjects()
        {
            if (dragAction.triggered)
            {
                Debug.Log("triggered");
                Physics.queriesHitBackfaces = false;
                RaycastHit hit;
                Physics.Raycast(transform.position, transform.forward, out hit, 5f);
                IMovable hitObject = hit.collider.gameObject.GetComponent<IMovable>();

                if (hitObject != null)
                {
                    Debug.Log("hit");
                    GameObject obj = hit.collider.gameObject;
                    obj.GetComponent<FixedJoint>().connectedBody = this.GetComponent<Rigidbody>();
                }

                else
                {
                    return;
                }

            }

            else
            {
                Debug.Log("button up");
            }
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            Rigidbody rb = hit.collider.attachedRigidbody;
            if(rb!=null && !rb.isKinematic && rb.mass < 2f)
            {
                rb.velocity = hit.moveDirection * pushForce;
            }
        }
    }

}

