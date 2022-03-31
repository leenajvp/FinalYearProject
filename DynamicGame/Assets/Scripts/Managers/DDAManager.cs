using Enemies;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

namespace DDA
{
    public class DDAManager : MonoBehaviour
    {
        [Header("All NPCs affected by DDA")]
        [SerializeField] public EnemyHealth[] npcHealth;
        [SerializeField] public EnemyBehaviourBase[] npcBheaviour;
        [SerializeField] public EnemyPools[] npcPools;
        [SerializeField] private GeneralCollectable[] HealthBoxes, BulletBoxes;

        [Header("Game Statistics")]
        public int currentShots, currentEHits, currentPHits, currentKills, currentsShots;
        public int totalShots, totaEHits, totalKills, totalDeaths, totalPhits;
        public int currentProgression = 0;
        public string events = "";

        [HideInInspector] public bool eAdjusted = false, hAdjusted = false, bAdjsuted = false, eLowered = false;
        [HideInInspector] public bool playerDead = false;


        private PlayerInventory pInventory;

        private bool adjusted = true;
        private bool adjusted2 = true;
        private int currentBullets;

        public bool gameComplete;

        void Start()
        {
            gameComplete = false;
            pInventory = FindObjectOfType<PlayerInventory>();

            totalShots = 0;
            totaEHits = 0;
            totalDeaths = 0;
            totalKills = 0;

            currentShots = 0;
            currentEHits = 0;
            currentPHits = 0;
            currentKills = 0;
            adjusted = false;
            adjusted2 = false;
        }

        void Update()
        {
            if (Time.timeScale == 0 && playerDead == true)
            {
                playerDead = false;
                totalDeaths++;
                pInventory.bullets = currentBullets;
                totalPhits += currentPHits;
                totalShots += currentShots;
                totaEHits += currentEHits;
                totalKills += currentKills;
                currentEHits = 0;
                currentPHits = 0;
                currentKills = 0;

                foreach (EnemyBehaviourBase npcs in npcBheaviour)
                {
                    npcs.Reset();
                }

                ManageGameDifficulty();
                events += "\n Player died " + Time.time.ToString();
            }

            if (gameComplete)
            {
                gameComplete = false;
                pInventory.bullets = currentBullets;
                totalPhits += currentPHits;
                totalShots += currentShots;
                totaEHits += currentEHits;
                totalKills += currentKills;
                gameComplete = false;
            }

            if (!adjusted && currentKills == npcBheaviour.Length / 2 && currentPHits < 20)
            {
                IncreaseEnemyShootingSpeed(0.2f);
                IncreaseEnemyDetection(20);
                Debug.Log("half enemies killed");
                events += "\n half enemies killed with less than 20 damage, enemy shoot frequency and detection increased ";
                adjusted = true;
            }

            if (!adjusted2 && currentShots == currentEHits && currentShots >= 30) // player is aiming well
            {
                IncreaseEnemyShootingSpeed(0.1f);
                IncreaseEnemyDetection(20);
                IncreaseEnemySpeed(10);
                IncreaseShootDistance(30);
                IncreaseRetreatDistance(30);
                Debug.Log("Good Aimer");
                events += "\n Good Aimer, first 30 shots all hit an enemy ";
                adjusted2 = true;
            }

            if (currentProgression == 2 && playerDead)
            {
                foreach (EnemyPools pool in npcPools)
                {
                    pool.stage0 = true;
                    ManageBulletCollectables(10);
                    ManageHealthCollectables(5);
                    events += "\n died in third stage, ammo and health collectables increaset 10,5 " + Time.time.ToString();

                    if (totalDeaths < 3)
                    {
                        npcPools[1].stage1 = true;

                        events += "\n Third stage with under 3 deaths " + Time.time.ToString();
                    }
                }
            }
        }

        public void GameComplete()
        {
            gameComplete = true;
        }

        public void SteamThroughDifficulty()
        {
            if (currentProgression < 2) // Player is very experienced
            {
                foreach (EnemyPools pool in npcPools)
                {
                    npcPools[1].stage1 = true;
                }

                ManageBulletCollectables(5);
                ManageHealthCollectables(5);
                IncreaseEnemyDetection(40);
                IncreaseEnemyShootingSpeed(0.1f);
                IncreaseEnemySpeed(5);
                IncreaseShootDistance(20);
                SetEnemyBulletDamage(1);
                Debug.Log("Difficult");
                events += "\n Player have rushed through to second stage, extra npc pool activated " + PlayerPrefs.GetInt("Progression").ToString() + "  " + Time.time.ToString();
            }
        }

        public void ManageGameDifficulty()
        {
            currentProgression = PlayerPrefs.GetInt("Progression");
            Debug.Log("hits: " + totaEHits + " kills: " + totalKills);

            if (currentBullets >= 10)
                pInventory.bullets = currentBullets;

            else
                pInventory.bullets = 10;

            // Increase Difficulty

            if (currentProgression == 1 && totalDeaths == 0 && currentPHits == 0)
            {
                foreach (EnemyPools pool in npcPools)
                {
                    pool.stage1 = true;
                }

                Debug.Log("Number of enemies increased for first pool as player did not get hit by first two npcs ");
                events += "\n Not hit on first part, first pool is added enemies " + Time.time.ToString();
            }

            if (totalDeaths == 0 && currentProgression >= 2) // Player is very experienced
            {
                npcPools[1].stage1 = true;
                npcPools[1].stage2 = true;
                ManageBulletCollectables(10);
                IncreaseEnemyDetection(40);
                IncreaseEnemyShootingSpeed(0.1f);
                IncreaseEnemySpeed(5);
                IncreaseShootDistance(20);
                SetEnemyBulletDamage(2);
                Debug.Log("Master");
                events += " \n Mastery level activated player have not died before 3rd stage, Diddiculty increased, ammo collectables increased, damage x2 " + Time.time.ToString();
            }

            if (totalDeaths == 1 && currentProgression > 2) // Player is very experienced
            {
                foreach (EnemyPools pool in npcPools)
                {
                    npcPools[1].stage1 = true;
                    npcPools[1].stage2 = true;
                }

                ManageBulletCollectables(5);
                ManageHealthCollectables(5);
                IncreaseEnemyDetection(40);
                IncreaseEnemyShootingSpeed(0.1f);
                IncreaseEnemySpeed(5);
                IncreaseShootDistance(20);
                SetEnemyBulletDamage(1);
                Debug.Log("Difficult");
                events += "\n Difficult level activated player died only once and is pass second stage, all collectables increased " + Time.time.ToString();
            }

            // Decrease Difficulty

            if (totalDeaths == 3)
            {
                foreach (EnemyPools pool in npcPools)
                {
                    pool.stage0 = true;
                }
                DecreaseEnemySpeed(10);
                ManageBulletCollectables(5);
                ManageHealthCollectables(5);
                SetEnemyBulletDamage(1);
                LowerEnemyShootingSpeed(0.2f);
                Debug.Log("died 3");
                events += "\n player have died 3 times, difficulty level lowered, collectables added 5  " + Time.time.ToString();
            }

            if (totalDeaths == 4 || totalDeaths == 6)
            {
                foreach (EnemyPools pool in npcPools)
                {
                    pool.stage0 = true;
                }
                ManageBulletCollectables(5);
                ManageHealthCollectables(5);
                LowerEnemyDetection(20);
                LowerEnemyShootingSpeed(0.5f);
                SetEnemyBulletDamage(1);
                DecreaseEnemySpeed(20);
                Debug.Log("died 4 or 6 ti");
                events += "\n Died 4 or 6 times, difficulty lowered,  " + Time.time.ToString();
            }

            if (currentProgression <= 1 && totalDeaths >= 3) // Player have not completed first section and dies more than twice
            {
                foreach (EnemyPools pool in npcPools)
                {
                    pool.stage0 = true;
                }
                ManageHealthCollectables(5);
                LowerEnemyShootingSpeed(0.3f);
                SetEnemyBulletDamage(1);
                Debug.Log("Baby");
                events += "\n Easiest level activated, health collectables +5  " + Time.time.ToString();
            }

            if (currentProgression <= 1 && totalDeaths > 1) // Player have not completed first section and dies more than twice
            {
                foreach (EnemyPools pool in npcPools)
                {
                    pool.stage0 = true;
                }
                ManageBulletCollectables(5);
                ManageHealthCollectables(5);
                SetEnemyBulletDamage(1);
                Debug.Log("Baby");
                events += "\n Easiest level activated, health collectables +5  " + Time.time.ToString();
            }
        }

        public void LowerEnemyHealth(float percentage)
        {
            foreach (EnemyHealth npcs in npcHealth)
            {
                if (npcs.currentHealth > 3)
                    npcs.currentHealth -= (npcs.currentHealth * percentage / 100); // decrease health by %
            }
        }

        private void IncreaseEnemySpeed(float percentage)
        {
            foreach (EnemyBehaviourBase npcs in npcBheaviour)
            {
                if (npcs.runSpeed < 7)
                    npcs.runSpeed += (npcs.runSpeed * percentage / 100); //increase run speed by %
            }
        }

        private void DecreaseEnemySpeed(float percentage)
        {
            foreach (EnemyBehaviourBase npcs in npcBheaviour)
            {
                if (npcs.runSpeed > npcs.data.runningSpeed)
                {
                    npcs.runSpeed -= (npcs.runSpeed * percentage / 100); //increase run speed by %
                }
            }
        }

        private void IncreaseEnemyShootingSpeed(float seconds)
        {
            foreach (EnemyBehaviourBase npcs in npcBheaviour)
            {
                if (npcs.shootFrequency > 0f)
                    npcs.shootFrequency -= seconds; // shorten shoot wait time by sec as long as its above min speed
            }
        }

        private void LowerEnemyShootingSpeed(float seconds)
        {
            foreach (EnemyBehaviourBase npcs in npcBheaviour)
            {
                if (npcs.shootFrequency < 1.5f)
                    npcs.shootFrequency += seconds; // increase shoot wait time by sec
            }

        }

        private void SetEnemyBulletDamage(float newValue)
        {
            foreach (EnemyBehaviourBase npcs in npcBheaviour)
                npcs.bDamage = newValue;
        }

        private void IncreaseEnemyDetection(float percentage)
        {
            foreach (EnemyBehaviourBase npcs in npcBheaviour)
                npcs.detectionDistance += (npcs.detectionDistance * percentage / 100);  // increase detection raycast distance by %
        }

        private void LowerEnemyDetection(float percentage)
        {
            foreach (EnemyBehaviourBase npcs in npcBheaviour)
            {
                if (npcs.detectionDistance > 20)
                    npcs.detectionDistance -= (npcs.detectionDistance * percentage / 100);  // lower detection raycast distance by %
            }
        }

        private void IncreaseShootDistance(float percentage)
        {
            foreach (EnemyBehaviourBase npcs in npcBheaviour)
                npcs.shootDist += (npcs.shootDist * percentage / 100);  // lower detection raycast distance by %
        }

        private void IncreaseRetreatDistance(float percentage)
        {
            foreach (EnemyBehaviourBase npcs in npcBheaviour)
                npcs.retreatDist += (npcs.retreatDist * percentage / 100);  // lower detection raycast distance by %
        }

        private void ManageHealthCollectables(int quantity)
        {
            foreach (GeneralCollectable h in HealthBoxes)
            {
                if (h.quantity < 30)
                    h.quantity += quantity; // increase health box quantity by X
            }

        }

        private void ManageBulletCollectables(int quantity)
        {
            foreach (GeneralCollectable b in BulletBoxes)
            {
                if (b.quantity < 45)
                    b.quantity += quantity; // increase bullet box quantity by X
            }

        }


        //EMAIL DATA


    }
}
