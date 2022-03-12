using Enemies;
using UnityEngine;

namespace DDA
{
    public class DDAManager : MonoBehaviour
    {
        [SerializeField] public EnemyHealth[] npcHealth;
        [SerializeField] public EnemyBehaviourBase[] npcBheaviour;

        public int currentHeadShots, currentEHits, currentPHits, currentKills, currentsShots;
        public int totalHeadShots, totalHits, totalKills, totalDeaths, totalShots;
        public bool playerDead;
        [SerializeField] private GameObject extraEnemies; // place hidden enemies that can be set active
        public int currentProgression = 0;
        public bool adjusted;

        void Start()
        {

            totalHeadShots = 0;
            totalHits = 0;
            totalDeaths = 0;
            totalKills = 0;

            currentHeadShots = 0;
            currentEHits = 0;
            currentPHits = 0;
            currentKills = 0;

            extraEnemies.SetActive(false);

            //get all enemies and add them to an array
        }

        void Update()
        {
            if (Time.timeScale == 0 && playerDead)
            {
                totalDeaths++;
                playerDead = false;
                totalHeadShots += currentHeadShots;
                totalHits += currentEHits;
                totalKills += currentKills;
                ShowStats();

                currentHeadShots = 0;
                currentEHits = 0;
                currentPHits = 0;
                currentKills = 0;
            }



            // if the player is not killed first enbemy pool easier faster
            // if player is on second enemy pool easier slower
            // final pool easier slowest
            currentProgression = PlayerPrefs.GetInt("Progression");

            if (currentProgression == 1 && totalDeaths == 0 && adjusted == false)
            {
                extraEnemies.SetActive(true);
                ManageEnemyHealth();
            }

        }

        private void ShowStats()
        {
            Debug.Log("hits: " + totalHits + " kills: " + totalKills);
        }

        // Different DDA measuring methods

        public void ManageEnemyHealth()
        {
            if (!adjusted)
            {
                foreach (EnemyHealth npcs in npcHealth)
                {
                    npcs.currentHealth += (int)(npcs.currentHealth * 20 / 100); // increase health by 20%
                }

                Debug.Log("Increased");
                adjusted = true;
            }
        }

        private void ManageEnemyMovementSpeed()
        {
            // If player is not getting hit
            foreach (EnemyBehaviourBase npcs in npcBheaviour)
            {
                npcs.data.walkingSpeed += (int)(npcs.data.walkingSpeed * 20 / 100); // increase health by 20%
                npcs.data.runningSpeed += (int)(npcs.data.runningSpeed * 20 / 100); // increase health by 20%
            }
        }

        private void ManageEnemyShootingSpeed()
        {
            //If player is not getting hit
            foreach (EnemyBehaviourBase npcs in npcBheaviour)
            {
                if (npcs.data.shootSpeed > 0.2f)
                    npcs.data.shootSpeed += (int)(npcs.data.shootSpeed -= 0.2f); ; // shorten shoot wait time by 0.2, this can be done up till the shoort timer is left with 0.2f
            }
        }

        private void ManageEnemyNumber()
        {
            // if player have not died before third pool, increase enemy number for final stage


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
