using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DDA;

namespace Player
{
    public class PlayerHealth : MonoBehaviour
    {
        public int health = 10;
        public int currentHealth;
        DDAManager ddaManager;

        private void Awake()
        {
            currentHealth = health;
        }

        private void Start()
        {
            if(ddaManager == null)
            {
                ddaManager = FindObjectOfType<DDAManager>();
            }

           
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
