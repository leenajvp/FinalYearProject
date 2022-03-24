using Enemies;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace DDA
{
    public class DDAManager : MonoBehaviour
    {
        [Header("All NPCs affected by DDA")]
        [SerializeField] public EnemyHealth[] npcHealth;
        [SerializeField] public EnemyBehaviourBase[] npcBheaviour;
        [SerializeField] public EnemyPools[] npcPools;
        [SerializeField] private GeneralCollectable[] HealthBoxes, BulletBoxes;
        [SerializeField] private Image tracker;
        private Renderer r;

        [Header("Game Statistics")]
        public int currentShots, currentEHits, currentPHits, currentKills, currentsShots;
        public int totalShots, totaEHits, totalKills, totalDeaths, totalPhits;
        public int currentProgression = 0;
        private string events = "";

        [HideInInspector] public bool eAdjusted = false, hAdjusted = false, bAdjsuted = false, eLowered = false;
        [HideInInspector] public bool playerDead = false;
        

        private PlayerInventory pInventory;

        private bool adjusted = true;
        private bool adjusted2 = true;
        private int currentBullets;

        void Start()
        {
            r = tracker.GetComponent<Renderer>();
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
               
            }

            if (!adjusted && currentKills == npcBheaviour.Length / 2 && currentPHits < 20)
            {
                IncreaseEnemyShootingSpeed(0.2f);
                IncreaseEnemyDetection(20);
                Debug.Log("half enemies killed");
                events += "\n half enemies killed ";
                adjusted = true;
                tracker.color = Color.gray;
            }

            if (!adjusted2 && currentShots == currentEHits && currentShots >= 30) // player is aiming well
            {
                IncreaseEnemyShootingSpeed(0.1f);
                IncreaseEnemyDetection(20);
                IncreaseEnemySpeed(10);
                IncreaseShootDistance(30);
                IncreaseRetreatDistance(30);
                Debug.Log("Good Aimer");
                events += "\n Good Aimer ";
                tracker.color = Color.yellow;
                adjusted2 = true;
            }

            if(currentProgression == 2 && playerDead)
            {
                foreach (EnemyPools pool in npcPools)
                {
                    pool.stage0 = true;
                    ManageBulletCollectables(5);
                    ManageHealthCollectables(5);
                    events += "\n died in thid stage " + Time.time.ToString();

                    if (totalDeaths < 3)
                    {
                        npcPools[1].stage1 = true;

                        events += "\n Third stage under 3 deaths " + Time.time.ToString();
                    }
                }
            }
        }

        public void ManageGameDifficulty()
        {
            currentProgression = PlayerPrefs.GetInt("Progression");
            Debug.Log("hits: " + totaEHits + " kills: " + totalKills);
            currentBullets = pInventory.bullets;

            // Increase Difficulty

            if (currentProgression == 1 && totalDeaths == 0 && currentPHits == 0)
            {
                foreach (EnemyPools pool in npcPools)
                {
                    pool.stage1 = true;
                }

                Debug.Log("Difficulty increase");
                events += "\n Not hit on first part " + Time.time.ToString();
            }

            if (currentProgression >= 2 && totalDeaths == 0) // Player is very experienced
            {
                npcPools[1].stage1 = true;
                npcPools[1].stage2 = true;
                IncreaseEnemyHealth(20);
                ManageBulletCollectables(20);
                IncreaseEnemyDetection(40);
                IncreaseEnemyShootingSpeed(0.1f);
                IncreaseEnemySpeed(5);
                IncreaseShootDistance(20);
                SetEnemyBulletDamage(2);
                tracker.color = Color.black;
                Debug.Log("Master");
                events += " \n Master level activated " + Time.time.ToString();

                if (currentPHits >= 20)
                {
                    ManageHealthCollectables(5);
                }
            }

            if (totalDeaths == 1 && currentProgression > 2) // Player is very experienced
            {
                foreach (EnemyPools pool in npcPools)
                {
                    npcPools[1].stage1 = true;
                    npcPools[1].stage2 = true;
                }

                ManageBulletCollectables(20);
                ManageHealthCollectables(5);
                IncreaseEnemyDetection(40);
                IncreaseEnemyShootingSpeed(0.1f);
                IncreaseEnemySpeed(5);
                IncreaseShootDistance(20);
                SetEnemyBulletDamage(1);
                tracker.color = Color.red;
                Debug.Log("Difficult");
                events += "\n Difficult level activated " + Time.time.ToString();
            }

            // Decrease Difficulty

            if (totalDeaths == 3)
            {
                DecreaseEnemySpeed(10);
                ManageBulletCollectables(5);
                ManageHealthCollectables(5);
                SetEnemyBulletDamage(1);
                LowerEnemyShootingSpeed(0.2f);
                Debug.Log("died 3");
                events += "\n player have died 3 times  " + Time.time.ToString();
                tracker.color = Color.magenta;
            }

            if (totalDeaths == 5 || totalDeaths == 10)
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
                Debug.Log("died 5 or 10");
                events += "\n Died 5 or 10 times " + Time.time.ToString();
                tracker.color = Color.green;
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
                events += "\n Easiest level activated " + Time.time.ToString();
                tracker.color = Color.white;
            }
        }

        private void IncreaseEnemyHealth(float percentage)
        {
            foreach (EnemyHealth npcs in npcHealth)
            {
                npcs.currentHealth += (npcs.currentHealth * percentage / 100); // increase health by %
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
                npcs.runSpeed += (npcs.runSpeed * percentage / 100); //increase run speed by %
            }
        }

        private void DecreaseEnemySpeed(float percentage)
        {
            foreach (EnemyBehaviourBase npcs in npcBheaviour)
            {
                if (npcs.runSpeed > 3f)
                {
                    npcs.runSpeed -= (npcs.runSpeed * percentage / 100); //increase run speed by %
                }
            }
        }

        private void IncreaseEnemyShootingSpeed(float seconds)
        {
            foreach (EnemyBehaviourBase npcs in npcBheaviour)
            {
                if (npcs.shootFrequency > -3f)
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
                npcs.bDamage += newValue; // increase shoot wait time by sec
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
                h.quantity += quantity; // increase health box quantity by X
        }

        private void ManageBulletCollectables(int quantity)
        {
            foreach (GeneralCollectable b in BulletBoxes)
                b.quantity += quantity; // increase bullet box quantity by X
        }

        const string kSenderEmailAddress = "gamestudydata@gmail.com";
        const string kSenderPassword = "ugStudy10";
        const string kReceiverEmailAddress = "gamestudydata@gmail.com";

        // Method 1: Direct message
        public void SendAnEmail()
        {
            // Create mail
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(kSenderEmailAddress);
            mail.To.Add(kReceiverEmailAddress);
            mail.Subject = "Email Title";
            mail.Body = "hits: " + totaEHits + "\n kills: " + totalKills + "\n shots: " + totalShots + "\n gameovers: " + totalDeaths + "\n kills: " + totalKills + "\n kills: " + totalKills + events + "\n total time " +Time.time;
            // mail.Attachments.Add(new Attachment("DynamicGame/screenshot.png"));

            // Setup server 
            SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
            smtpServer.Port = 587;
            smtpServer.Credentials = new NetworkCredential(
                kSenderEmailAddress, kSenderPassword) as ICredentialsByHost;
            smtpServer.EnableSsl = true;
            ServicePointManager.ServerCertificateValidationCallback =
                delegate (object s, X509Certificate certificate,
                X509Chain chain, SslPolicyErrors sslPolicyErrors)
                {
                    Debug.Log("Email success!");
                    return true;
                };

            // Send mail to server, print results
            try
            {
                smtpServer.Send(mail);
            }
            catch (System.Exception e)
            {
                Debug.Log("Email error: " + e.Message);
            }
            finally
            {
                Debug.Log("Email sent!");
            }
        }
    }
}
