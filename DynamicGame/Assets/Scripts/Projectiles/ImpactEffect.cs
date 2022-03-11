using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactEffect : MonoBehaviour
{
    private void Update()
    {
        Destroy(gameObject, 0.2f);
    }
}

