using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableFob : MonoBehaviour
{

    public bool AllowFog = false;

    private bool FogOn;

    private void OnPreRender()
    {
        RenderSettings.fog = AllowFog;

    }

    private void OnPostRender()
    {
        RenderSettings.fog = FogOn;
    }
}

