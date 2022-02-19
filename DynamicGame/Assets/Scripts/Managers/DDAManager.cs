using Enemies;
using UnityEngine;

namespace DDA
{
    public class DDAManager : MonoBehaviour
    {
        [SerializeField] public EnemyData enemyData;

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

        [SerializeField] private GameObject[] EnemyPools;
        public int currentProgression = 0;


        bool gameOver;

        // PlayerPrefs DifLevel , FirstAttempt, Progression
        void Start()
        {
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

            // if the player is not killed first enbemy pool easier faster
            // if player is on second enemy pool easier slower
            // final pool easier slowest
        }

        private void ShowStats()
        {
            Debug.Log("hits: " + totalHits + " kills: " + totalKills);
        }

        // Different DDA measuring methods

        public void ManageEnemyHealth()
        {
            // If player is not loosing health

            enemyData.health += (int)(enemyData.health * 20 / 100);
            Debug.Log("Increased");
        }

        private void ManageEnemyMovementSpeed()
        {
            // If player is not getting hit
        }

        private void ManageEnemyShootingSpeed()
        {
            //If player is not getting hit
        }

        private void ManageEnemyNumber()
        {

        }

        private void ManageEnemyDetection()
        {

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
