using Bullets;
using Cinemachine;
using DDA;
using UnityEngine;
using UnityEngine.InputSystem;
using Enemies;
using System.Collections;

namespace Player
{
    [RequireComponent(typeof(PlayerController), typeof(PlayerInput), typeof(CharacterController))]
    [RequireComponent(typeof(PlayerInventory), typeof(PlayerHealth))]
    [RequireComponent(typeof(AudioSource))]
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
        [SerializeField] private ParticleSystem impact;
        private float shootTimer = 0.0f;

        [Header("Interactions")]
        public bool interacting = false;
        public bool availableInteraction = false;
        [SerializeField] private GameObject interactHUD;

        [Header("Sound Effects")]
        [SerializeField] private AudioSource collectSound;

        [Header("UI")]
        [SerializeField] private GameObject pauseMenu;
        [SerializeField] private GameObject map;
        [SerializeField] private GameObject LockUI;

        public bool isDisguised = false;

        private Transform cameraTransform;
        private CharacterController controller;
        private PlayerInput playerInput;
        private InputAction moveAction;
        private InputAction lookAction;
        private InputAction jumpAction;
        private InputAction runAction;
        private InputAction shootAction;
        [HideInInspector] public InputAction interactAction;
        private InputAction pauseGame;
        private InputAction openMap;
#if UNITY_EDITOR
        private InputAction godMode;
        #endif
        private PlayerInventory inventory;
        private DDAManager ddaManager;
        private PlayerHealth pHealth;

        LayerMask layer_mask;
        int layerMask;

        private void Awake()
        {
            layer_mask = LayerMask.GetMask( "Default", "Environment", "Objects", "Ground");
            controller = GetComponent<CharacterController>();
            playerInput = GetComponent<PlayerInput>();
            moveAction = playerInput.actions["Move"];
            lookAction = playerInput.actions["Look"];
            jumpAction = playerInput.actions["Jump"];
            runAction = playerInput.actions["Run"];
            shootAction = playerInput.actions["Shoot"];
            interactAction = playerInput.actions["Interact"];
            pauseGame = playerInput.actions["Pause"];
            openMap = playerInput.actions["Map"];
#if UNITY_EDITOR
            godMode = playerInput.actions["GodMode"];
#endif

            pHealth = GetComponent<PlayerHealth>();
            inventory = GetComponent<PlayerInventory>();
            collectSound = GetComponent<AudioSource>();

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
            isDisguised = false;
        }

        void Update()
        {

#if UNITY_EDITOR
            if (godMode.triggered)
            {
                GetComponent<PlayerHealth>().currentHealth += 10;
                inventory.bullets += 10;
            }
#endif

            Interact();
            Jump();
            Move();

            if (pauseGame.triggered)
            {
                PauseGame();
                DisablePlayer();

                if (pauseMenu.activeSelf)
                {
                    pauseMenu.SetActive(false);
                    openMap.Enable();
                }
                else
                {
                    pauseMenu.SetActive(true);
                    openMap.Disable();
                    foreach (Transform child in LockUI.transform)
                        child.gameObject.SetActive(false);
                }

            }

            if (openMap.triggered)
            {
                DisablePlayer();

                if (map.activeSelf)
                {
                    map.SetActive(false);
                    pauseGame.Enable();
                }

                else
                {
                    map.SetActive(true);
                    pauseGame.Disable();
                }
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
            if (Time.time < shootTimer + 0.01f)
                return;

            shootTimer = Time.time;


            if (inventory.bullets > 0)
            {
                ddaManager.currentsShots++;

                muzzleFlash.Play();

                RaycastHit hit;
               // RaycastHit hit;
                //QueryTriggerInteraction.Ignore
                if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, 60, layer_mask))
                {
                    //shootPoint.LookAt(hit.point);
                  //  Physics.Raycast(shootPoint.position, shootPoint.forward, out hit, 60, layer_mask);

                    if (hit.collider.gameObject)
                    {
                        var newImpact = Instantiate(impact, hit.point, Quaternion.LookRotation(hit.normal));
                        Destroy(newImpact, 0.5f);

                        EnemyHealth enemy = hit.collider.gameObject.GetComponent<EnemyHealth>();

                        if (enemy != null)
                        {
                            ddaManager.currentEHits++;
                            enemy.currentHealth -= 1;
                            enemy.gameObject.GetComponent<EnemyBehaviourBase>().isHit = true;
                            enemy.ChangeMaterial();

                            if (isDisguised && !enemy.explorationNPC)
                                isDisguised = false;
                        }
                    }
                }

                ddaManager.currentShots++;
                inventory.bullets--;
            }
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

                if (interactive != null && interactive.notCompleted)
                {
                    interactHUD.SetActive(true);

                    if (interactAction.triggered)
                    {
                        interactive.GetInteraction();
                        collectSound.Play();
                    }
                }

                else if (collectable != null && collectable.isInventoryItem)
                {
                    interactHUD.SetActive(true);
                    IQuestItems qItem = hit.collider.gameObject.GetComponent<IQuestItems>();

                    if (interactAction.triggered)
                    {
                        collectSound.Play();
                        interactHUD.SetActive(false);
                        collectable.collected = true;
                        inventory.AddItem(qItem);
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
        }

        private void OnTriggerEnter(Collider other)
        {
            GeneralCollectable collectable = other.gameObject.GetComponent<GeneralCollectable>();

            if (collectable != null)
            {
                collectSound.Play();
                if (collectable.type == GeneralCollectable.CollectableType.Bullets)
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

        public void DisableMenus()
        {
            if (Time.timeScale == 0)
            {
                openMap.Disable();
                pauseGame.Disable();
            }

            else
            {
                openMap.Enable();
                pauseGame.Enable();
            }
        }


    }
}

