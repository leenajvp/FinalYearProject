using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StaticCameras : MonoBehaviour
{
    [SerializeField] private int cameraNum = 0;
    [SerializeField] private CameraManager ViewManager;
    private PlayerInput inputSystem;

    private void OnTriggerEnter(Collider other)
    {
    }

    private void OnMouseDown()
    {
        Debug.Log("click");
        ViewManager.SelectCamera(cameraNum);
    }
}
