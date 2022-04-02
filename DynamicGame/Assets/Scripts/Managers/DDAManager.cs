using Enemies;
using System.Collections.Generic;
using UnityEngine;

namespace DDA
{
    public class DDAManager : MonoBehaviour
    {
        [Header("All NPCs affected by DDA")]
        public List<EnemyHealth> npcHealth = new List<EnemyHealth>();
        public List<EnemyBehaviourBase> npcBehaviour = new List<EnemyBehaviourBase>();
        public List<EnemyPools> npcPools = new List<EnemyPools>();
        [SerializeField] private List<GeneralCollectable> healthBoxes, bulletBoxes = new List<GeneralCollectable>();

        [Header("Game Statistics")]
        public int currentShots, currentEHits, currentPHits, currentKills, currentsShots;
        public int totalShots, totaEHits, totalKills, totalDeaths, totalPhits;
        public int currentProgression = 0;
        public string events = "";

       // [HideInInspector] public bool eAdjusted = false, hAdjusted = false, bAdjsuted = false, eLowered = false;
        [HideInInspector] public bool playerDead = false;

        private PlayerInventory pInventory;

        private bool masterActivated = false;
        private bool difficultActivated = false;
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

                //foreach (EnemyBehaviourBase npcs in npcBehaviour)
                //{
                //    npcs.Reset();
                //}

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

            if (!adjusted && currentKills > 1 && currentKills == npcBehaviour.Count / 2 && currentPHits < 20)
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
                    events += "\n died in third stage, ammo and health collectables increased 10,5 " + Time.time.ToString();

                    if (totalDeaths < 3)
                    {
                        npcPools[1].available1 = true;
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
                masterActivated = true;
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

            if (totalDeaths <= 3 && currentProgression >= 2) // Player is very experienced
            {
                if (!masterActivated)
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
                }

                SetEnemyBulletDamage(1);
                Debug.Log("Difficult");
                events += "\n Difficult level activated player died only once and is pass second stage, all collectables increased " + Time.time.ToString();
            }

            // Decrease Difficulty

            if(totalDeaths == 2)
            {
                ManageBulletCollectables(5);
                ManageHealthCollectables(5);
            }

            if (totalDeaths == 3)
            {
                foreach (EnemyPools pool in npcPools)
                {
                    pool.stage0 = true;

                    if(masterActivated || difficultActivated)
                    {
                        npcPools[1].available2 = true;
                        npcPools[1].stage2 = true;
                    }
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

            if (currentProgression <= 1 && totalDeaths >= 2) // Player have not completed first section and dies more than twice
            {
                foreach (EnemyPools pool in npcPools)
                {
                    pool.stage0 = true;
                }
                ManageHealthCollectables(5);
                LowerEnemyShootingSpeed(0.3f);
                SetEnemyBulletDamage(1);
                Debug.Log("Baby");
                events += "\n Easiest level activated " + Time.time.ToString();
            }

            if (currentProgression <= 1 && totalDeaths > 2) // Player have not completed first section and dies more than twice
            {
                foreach (EnemyPools pool in npcPools)
                {
                    pool.stage0 = true;
                }
                ManageBulletCollectables(5);
                ManageHealthCollectables(5);
                SetEnemyBulletDamage(1);
                Debug.Log("Baby");
                events += "\n Easiest level activated  " + Time.time.ToString();
            }
        }

        public void LowerEnemyHealth(float percentage)
        {
            npcHealth.ForEach(npc =>
            {
                if (npc.currentHealth > 3)
                {
                    npc.currentHealth -= (npc.currentHealth * percentage / 100); // decrease all npc health by %
                }
            });

            events += "\n NPCs health decreased by " + percentage + "% " + Time.time.ToString();
        }

        private void IncreaseEnemySpeed(float percentage)
        {
            npcBehaviour.ForEach(npc =>
            {
                if (npc.runSpeed < 7)
                {
                    npc.runSpeed += (npc.runSpeed * percentage / 100); // increase enemy speed by %
                }
            }); 

            events += "\n NPCs health increased by " + percentage + "% " + Time.time.ToString();
        }

        private void DecreaseEnemySpeed(float percentage)
        {
            npcBehaviour.ForEach(npc =>
            {
                if (npc.runSpeed < npc.data.runningSpeed)
                {
                    npc.runSpeed -= (npc.runSpeed * percentage / 100);  // decrease speed by %
                }
            });

            events += "\n NPCs speed decreased by " + percentage + "% " + Time.time.ToString();
        }

        private void IncreaseEnemyShootingSpeed(float seconds)
        {
            npcBehaviour.ForEach(npc =>
            {
                if (npc.shootFrequency > 0.1f)
                {
                    npc.shootFrequency -= seconds; // shorten shoot wait time by sec as long as its above min speed
                }
            });

            events += "\n NPCs shoot speed increased by " + seconds + " " + Time.time.ToString();
        }

        private void LowerEnemyShootingSpeed(float seconds)
        {
            npcBehaviour.ForEach(npc =>
            {
                if (npc.shootFrequency < 1.5f)
                {
                    npc.shootFrequency += seconds; // increase shoot wait time by sec
                }
            });

            events += "\n NPCs shoot speed increased by " + seconds + " " + Time.time.ToString();
        }

        private void SetEnemyBulletDamage(float newValue)
        {
            npcBehaviour.ForEach(npc => npc.bDamage = newValue);
            events += "\n NPC bullet damage set: " + newValue + " " + Time.time.ToString();
        }

        private void IncreaseEnemyDetection(float percentage)
        {
            npcBehaviour.ForEach(npc => npc.detectionDistance += (npc.detectionDistance * percentage / 100)); // increase detection raycast distance by %

            events += "\n NPC detection distance increased by " + percentage + "% " + Time.time.ToString();
        }

        private void LowerEnemyDetection(float percentage)
        {
            npcBehaviour.ForEach(npc =>
            {
                if (npc.detectionDistance > 20)
                {
                    npc.detectionDistance -= (npc.detectionDistance * percentage / 100); // increase shoot wait time by sec
                }
            });

            events += "\n NPC detection distance lowered by " + percentage + "% " + Time.time.ToString();
        }

        private void IncreaseShootDistance(float percentage)
        {
            npcBehaviour.ForEach(npc =>
            {
                if (npc.shootFrequency < 1.5f)
                {
                    npc.shootDist += (npc.shootDist * percentage / 100); // increase shoot distance by %
                }
            });
            events += "\n NPC shoot distance increased by " + percentage + "% " + Time.time.ToString();
        }

        private void ManageHealthCollectables(int quantity)
        {
            healthBoxes.ForEach(h =>
            {
                if (h.quantity < 30)
                {
                    h.quantity += quantity;
                }
            });

            events += "\n Health collectables increased by " + quantity + " " + Time.time.ToString();
        }

        private void ManageBulletCollectables(int quantity)
        {
            bulletBoxes.ForEach(b =>
            {
                if (b.quantity < 45)
                {
                    b.quantity += quantity;
                }
            });

            events += "\n Ammo collectables increased by " + quantity + " " + Time.time.ToString();
        }
    }
}
