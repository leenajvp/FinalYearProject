using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DDA;

namespace Player
{
    public class PlayerHealth : MonoBehaviour
    {
        public float health = 10;
        public float currentHealth;
        DDAManager ddaManager;

        private void Start()
        {
            if(ddaManager == null)
            {
                ddaManager = FindObjectOfType<DDAManager>();
            }

            currentHealth = health;
        }

        private void Update()
        {

            if (currentHealth <= 0)
            {
                ddaManager.playerDead = true;
                Time.timeScale = 0;
                currentHealth = health;
            }
        }
    }
}
