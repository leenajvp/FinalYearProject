using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DDA;

public class EnemyHealth : MonoBehaviour
{
    public float health = 10.0f;
    public float currentHealth = 10.0f;
    DDAManager ddaManager;

    private void Start()
    {
        if (ddaManager == null)
        {
            ddaManager = FindObjectOfType<DDAManager>();
        }

        currentHealth = health;

    }

    // Update is called once per frame
    void Update()
    {
        if(currentHealth <= 0)
        {
            ddaManager.currentKills++;
            gameObject.SetActive(false);
        }
    }
}
