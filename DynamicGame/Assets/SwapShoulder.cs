using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwapShoulder : MonoBehaviour
{
    //[SerializeField] private PlayerInput playerInput;
    //[SerializeField] private int newPriority = 10;
    //private InputAction swapShoulder;
    //private CinemachineVirtualCamera vCamera;
    //private bool left;


    //private void Awake()
    //{
    //    left = false;
    //    vCamera = GetComponent<CinemachineVirtualCamera>();
    //    swapShoulder = playerInput.actions["Swap"];
    //}

    //private void OnEnable()
    //{
    //    swapShoulder.performed += _ => Change();
    //}

    //private void OnDisable()
    //{
    //    swapShoulder.performed -= _ => Change();
    //}

    //private void Change()
    //{
    //    var thirdperson = vCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();

    //    if (!left)
    //    {
    //        thirdperson.CameraSide = 10f;
    //        left = true;
    //    }

    //    else
    //    {
    //        thirdperson.CameraSide = 90f;
    //        left = false;
    //    }
    //}
}
