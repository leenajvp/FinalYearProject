using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class SwitchCamera : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private int newPriority = 10;
    [SerializeField] private Canvas tpCanvas;
    [SerializeField] private Canvas aimCanvas;
    private InputAction aimAction;
    private CinemachineVirtualCamera vCamera;


    private void Awake()
    {
        vCamera = GetComponent<CinemachineVirtualCamera>();
        aimAction = playerInput.actions["Aim"];
    }

    private void OnEnable()
    {
        aimAction.performed += _ => StartAim();
        aimAction.canceled += _ => CancelAim();
    }

    private void OnDisable()
    {
        aimAction.performed -= _ => StartAim();
        aimAction.canceled -= _ => CancelAim();
    }

    private void StartAim()
    {
        vCamera.Priority += newPriority;
        aimCanvas.enabled = true;
        tpCanvas.enabled = false;
    }

    private void CancelAim()
    {
        vCamera.Priority -= newPriority;
        aimCanvas.enabled = false;
        tpCanvas.enabled = true;
    }
}
