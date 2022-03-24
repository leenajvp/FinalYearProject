using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DDA;

namespace Player
{
    public class PlayerHealth : MonoBehaviour
    {
        public float health = 10;
        public float currentHealth;
       [SerializeField] private DDAManager ddaManager;

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
                gameObject.SetActive(false);
                ddaManager.playerDead = true;
                currentHealth = health;
            }
        }
    }
}
