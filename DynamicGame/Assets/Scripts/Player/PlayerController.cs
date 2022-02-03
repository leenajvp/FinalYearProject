using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Bullets;
using DDA;


namespace Player
{
    [RequireComponent(typeof(PlayerController), typeof(PlayerInput), typeof(PlayerController))]

    public class PlayerController : MonoBehaviour
    {
        [Header("Player Movement Settings")]
        [SerializeField] private Vector3 playerVelocity;
        [SerializeField] private bool groundedPlayer;
        [SerializeField] private float playerSpeed = 2.0f;
        [SerializeField] private float rotSpeed = 5.0f;


        [SerializeField] private GameObject bullet;
        [SerializeField] private Transform shootPoint;
        [SerializeField] private Transform bulletParent;
        [SerializeField] private ObjectPool bulletPool;

        private float jumpHeight = 1.0f;
        private float gravityValue = -9.81f;

        private Transform cameraTransform;
        private CharacterController controller;
        private PlayerInput playerInput;
        private InputAction moveAction;
        private InputAction jumpAction;
        private InputAction shootAction;

        private DDAManager ddaManager;

        private void Awake()
        {
            
            controller = GetComponent<CharacterController>();
            playerInput = GetComponent<PlayerInput>();
            moveAction = playerInput.actions["Move"];
            jumpAction = playerInput.actions["Jump"];
            shootAction = playerInput.actions["Shoot"];

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
        }

        void Update()
        {
            Vector3 forward = transform.TransformDirection(Vector3.forward) * 100;
            Debug.DrawRay(transform.position, forward, Color.green);

            groundedPlayer = controller.isGrounded;
            if (groundedPlayer && playerVelocity.y < 0)
            {
                playerVelocity.y = 0f;
            }

            Vector2 input = moveAction.ReadValue<Vector2>();
            Vector3 move = new Vector3(input.x, 0, input.y);
            move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;
            move.y = 0f;
            controller.Move(move * Time.deltaTime * playerSpeed);

            // Changes the height position of the player.
            if (jumpAction.triggered && groundedPlayer)
            {
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            }

            playerVelocity.y += gravityValue * Time.deltaTime;
            controller.Move(playerVelocity * Time.deltaTime);

            //Rotate with camera
            Quaternion targetRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotSpeed * Time.deltaTime);

        }

        private void Shoot()
        {
            ddaManager.currentsShots++;

            RaycastHit hit;
            GameObject g = bulletPool.GetObject();
            g.transform.position = shootPoint.position;
            g.transform.rotation = shootPoint.rotation;
            g.SetActive(true);

            BulletController bulletController = g.GetComponent<BulletController>();

            if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, Mathf.Infinity))
            {
                bulletController.target = hit.point;
                bulletController.hit = true;
            }

            else
            {
                bulletController.target = cameraTransform.position + cameraTransform.forward * 25;
                bulletController.hit = false;
            }
        }

    }
}
