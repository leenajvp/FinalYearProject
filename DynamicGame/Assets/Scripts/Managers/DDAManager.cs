using UnityEngine;
using Player;
using System.Collections.Generic;

namespace DDA
{
    public class DDAManager : MonoBehaviour
    {
        public int currentHeadShots;
        public int currentHits;
        public int currentKills;
        public int currentsShots;

        public bool playerDead;

        public static int totalHeadShots;
        public static int totalHits;
        public static int totalKills;
        public static int totalDeaths;
        public static int totalShots;

        [SerializeField] private GameObject player;
        private int pHealth;
        [SerializeField] List<EnemyPools> ePools = new List<EnemyPools>();

        private List<EnemyBehaviour> enemies = new List<EnemyBehaviour>();

        bool gameOver;

        private bool healthIncreased = false;


        // PlayerPrefs DifLevel , FirstAttempt, Progression


        void Start()
        {
            pHealth = player.GetComponent<PlayerHealth>().currentHealth;

            if (PlayerPrefs.GetInt("FirstAttempt") == 0 || PlayerPrefs.GetInt("Progression") == 0)
            {
                totalHeadShots = 0;
                totalHits = 0;
                totalDeaths = 0;
                totalKills = 0;
            }

            currentHeadShots = 0;
            currentHits = 0;
            currentKills = 0;
        }

        void Update()
        {
            if (Time.timeScale == 0 && playerDead)
            {
                totalDeaths++;
                totalHeadShots += currentHeadShots;
                totalHits += currentHits;
                totalKills += currentKills;
                playerDead = false;
                ShowStats();

                currentHeadShots = 0;
                currentHits = 0;
                currentKills = 0;
            }

            ManageEnemyHealth();
        }

        private void ShowStats()
        {
            Debug.Log("hits: " + totalHits + " kills: " + totalKills);
        }

        // Methods to manager Difficulty

        public void ManageEnemyDetection()
        {
            // if player is not getting detected by X kill
        }

        public void ManageEnemyHealth()
        {
            // if player has more hits than damage

            if(pHealth == currentHits && !healthIncreased)
            {
                foreach(EnemyPools pool in ePools)
                {
                    pool.AddEnemyHealth();
                    healthIncreased = true;
                }
            }
        }

        public void ManageEnemyNumber()
        {
            // If player has triple hits to damage
        }

        public void ManageEnemyStrenght()
        {
            // If player has double hits to damage
        }

        private void AdjustEnemyHealth()
        {
            if (PlayerPrefs.GetInt("DifLevel") == 5 || PlayerPrefs.GetInt("DifLevel") == 0)
                return;

            else if (PlayerPrefs.GetInt("DifLevel") == 4)
            {

            }

            else if (PlayerPrefs.GetInt("DifLevel") == 3)
            {

            }

            else if (PlayerPrefs.GetInt("DifLevel") == 2)
            {

            }

            else if (PlayerPrefs.GetInt("DifLevel") == 4)
            {

            }
        }
    }
}
