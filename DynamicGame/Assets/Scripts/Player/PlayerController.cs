using Bullets;
using DDA;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

namespace Player
{
    [RequireComponent(typeof(PlayerController), typeof(PlayerInput), typeof(CharacterController))]
    [RequireComponent(typeof(PlayerInventory), typeof (PlayerHealth))]

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
        [SerializeField] private float rayDetectionDistance = 5f;
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
        [SerializeField] private GameObject interactHUD;

        [Header("UI")]
        [SerializeField] private GameObject pauseMenu;
        [SerializeField] private GameObject map;

        private Transform cameraTransform;
        private CharacterController controller;
        private PlayerInput playerInput;
        private InputAction moveAction;
        private InputAction lookAction;
        private InputAction jumpAction;
        private InputAction shootAction;
        public  InputAction interactAction;
        private InputAction pauseGame;
        private InputAction openMap;

        private PlayerInventory inventory;
        private DDAManager ddaManager;
        private PlayerHealth pHealth;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            playerInput = GetComponent<PlayerInput>();
            moveAction = playerInput.actions["Move"];
            lookAction = playerInput.actions["Look"];
            jumpAction = playerInput.actions["Jump"];
            shootAction = playerInput.actions["Shoot"];
            interactAction = playerInput.actions["Interact"];
            pauseGame = playerInput.actions["Pause"];
            openMap = playerInput.actions["Map"];

            pHealth = GetComponent<PlayerHealth>();
            inventory = GetComponent<PlayerInventory>();

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
            interactHUD.SetActive(false);
            map.SetActive(false);
            pauseMenu.SetActive(false);
        }

        void Update()
        {
            Interact();
            Jump();
            Move();
          //  DisablePlayer();

            // Check for GamePause and Map inputs

            if (pauseGame.triggered)
            {
                PauseGame();

                if (pauseMenu.activeSelf)
                    pauseMenu.SetActive(false);

                else
                    pauseMenu.SetActive(true);
            }

            if (openMap.triggered)
            {
                PauseGame();

                if (map.activeSelf)
                    map.SetActive(false);

                else
                    map.SetActive(true);
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
            if (inventory.bullets > 0)
            {
                inventory.bullets--;
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
            // ddaManager.ManageEnemyHealth();

        }

        public void DisablePlayer()
        {
            if (interacting)
            {
                Camera.main.GetComponent<CinemachineBrain>().enabled = false;
                moveAction.Disable();
                jumpAction.Disable();
                lookAction.Disable();
                shootAction.Disable();
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }

            else
            {
                Camera.main.GetComponent<CinemachineBrain>().enabled = true;
                moveAction.Enable();
                jumpAction.Enable();
                lookAction.Enable();
                shootAction.Enable();
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        // Check if raycast hit on interactive objects
        private void Interact()
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.forward, out hit, rayDetectionDistance))
            {
                Iinteractive interactive = hit.collider.gameObject.GetComponent<Iinteractive>();
                ICollectable collectable = hit.collider.gameObject.GetComponent<ICollectable>();

                if (interactive != null && interactive.available)
                {
                    interactHUD.SetActive(true);

                    if (interactAction.triggered)
                    {
                        interactive.GetInteraction();
                    }
                }

                else if (collectable != null && collectable.isInventoryItem)
                {
                    interactHUD.SetActive(true);
                    IQuestItems code = hit.collider.gameObject.GetComponent<IQuestItems>();

                    if (interactAction.triggered)
                    {
                        interactHUD.SetActive(false);
                        collectable.collected = true;
                        inventory.AddItem(code);
                    }
                }

                else
                {
                    interactHUD.SetActive(false);
                    return;
                }
            }

            else if (hit.collider == null)
            {
                interactHUD.SetActive(false);
                return;
            }
                
        }

        // Add push force to controller
        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            Rigidbody rb = hit.collider.attachedRigidbody;

            if (rb != null && !rb.isKinematic && rb.mass < 2f)
            {
                rb.velocity = hit.moveDirection * pushForce;
            }

            GeneralCollectable collectable = hit.gameObject.GetComponent<GeneralCollectable>();

            if (collectable != null)
            {
                if(collectable.type == GeneralCollectable.CollectableType.Bullets)
                {
                    inventory.bullets += collectable.quantity;
                }

                else
                {
                    pHealth.currentHealth += collectable.quantity;
                }

                collectable.Collect();
            }
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public void PauseGame()
        {
            if (Time.timeScale == 1)
            {
                interacting = true;
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            else
            {
                interacting = false;
                Time.timeScale = 1;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

        }
    }
}

