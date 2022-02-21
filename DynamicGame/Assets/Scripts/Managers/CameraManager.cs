using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    [Header("Cameras")]
    public Camera defaultCamera;
    public Camera[] objCameras;

    [Header("Lights")]
    public Light defaultLight, defaultLightCenter = null;
    [SerializeField] private Light[] objLights = null;

    [Header("Force camera back to default")]
    public bool camIsForced = false;

    private void Update()
    {
        if (camIsForced)
        {
            defaultCamera.enabled = true;
            defaultLight.enabled = true;
            defaultLightCenter.enabled = true;

            for (var i = 0; i < objLights.Length; i++)
            {
                objLights[i].enabled = false;
            }

            for (var i = 0; i < objCameras.Length; i++)
            {
                objCameras[i].enabled = false;
            }

            camIsForced = false;
        }
    }

    public void SelectCamera(int num)
    {
        if (defaultCamera.enabled == true)
        {
            for (var i = 0; i < objCameras.Length; i++)
            {
                if (i == num)
                {
                    objCameras[i].enabled = true;
                }
                else
                {
                    objCameras[i].enabled = false;
                }
            }
            Cursor.visible = false;
            defaultCamera.enabled = false;
        }

        else
        {
            for (var i = 0; i < objCameras.Length; i++)
            {
                objCameras[i].enabled = false;
            }
            defaultCamera.enabled = true;
        }
    }
}
