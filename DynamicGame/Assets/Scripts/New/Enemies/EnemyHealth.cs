using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DDA;

public class EnemyHealth : MonoBehaviour
{
    public float health = 10.0f;
    DDAManager ddaManager;

    private void Start()
    {
        if (ddaManager == null)
        {
            ddaManager = FindObjectOfType<DDAManager>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(health <= 0)
        {
            ddaManager.currentKills++;
            Destroy(gameObject);
        }
    }
}
