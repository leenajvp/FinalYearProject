using DDA;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPools : MonoBehaviour
{
    [Header("Enemy Pool settings")]
    [SerializeField] private DDAManager ddaManager;
    public int checkPointID = 0;
    public int defeats = 0;
    public int originalCount;
    public int currentCount;

    [Header("Managing NPC numbers")]
    [SerializeField] public List<EnemyHealth> enemyPool = new List<EnemyHealth>();
    [SerializeField] private List<EnemyHealth> extraEnemies = new List<EnemyHealth>();
    [SerializeField] private List<EnemyHealth> extraEnemies2 = new List<EnemyHealth>();
    public bool stage1 = false;
    public bool stage2 = false;
    public bool stage0 = false;

    [Header("Resetting Collectables")]
    [SerializeField] public List<GeneralCollectable> areaCollectables = new List<GeneralCollectable>();

    [Header("UI")]
    [SerializeField] private GameObject savingHUD;


    private List<Enemies.EnemyBehaviourBase> behaviourPool = new List<Enemies.EnemyBehaviourBase>();

    private void Start()
    {
        currentCount = enemyPool.Count;
        originalCount = enemyPool.Count;

        if (ddaManager == null)
            ddaManager = FindObjectOfType<DDAManager>();

        foreach (EnemyHealth enemy in extraEnemies)
        {
            enemy.gameObject.SetActive(false);
        }

        foreach (EnemyHealth enemy in extraEnemies2)
        {
            enemy.gameObject.SetActive(false);
        }

        enemyPool.ForEach(enemy => behaviourPool.Add(enemy.GetComponent<Enemies.EnemyBehaviourBase>()));
    }

    private void Update()
    {
        if (stage0 == true)
        {
            for (int i = originalCount; i < enemyPool.Count; i++)
            {
                enemyPool[i].gameObject.SetActive(false);
            }

            enemyPool.RemoveAll(enemy => !enemy.gameObject.activeSelf);
            currentCount = enemyPool.Count;
            stage0 = false;
        }

        if (stage1 == true)
        {
            for (int i = 0; i < extraEnemies.Count; i++)
            {
                extraEnemies[i].gameObject.SetActive(true);
                enemyPool.Add(extraEnemies[i]);
                behaviourPool.Add(extraEnemies[i].GetComponent<Enemies.EnemyBehaviourBase>());
            }
            currentCount = enemyPool.Count;
            stage1 = false;
        }

        if (stage2 == true)
        {
            for (int i = 0; i < extraEnemies2.Count; i++)
            {
                extraEnemies2[i].gameObject.SetActive(true);
                enemyPool.Add(extraEnemies2[i]);
                behaviourPool.Add(extraEnemies[i].GetComponent<Enemies.EnemyBehaviourBase>());
            }
            currentCount = enemyPool.Count;
            stage2 = false;
        }
    }

    public void UpdateProgress()
    {
        for (int i = 0; i < enemyPool.Count; i++)
        {
            if (enemyPool[i].gameObject.activeInHierarchy)
            {
                defeats++;
                break;
            }
        }

        if (defeats >= currentCount && PlayerPrefs.GetInt("Progression") < checkPointID)
        {
            savingHUD.SetActive(true);
            PlayerPrefs.SetInt("Progression", checkPointID);
            ddaManager.events += "\n Player progressions " + PlayerPrefs.GetInt("Progression").ToString() + "  " + Time.time.ToString();
            ddaManager.ManageGameDifficulty();
            Debug.Log(PlayerPrefs.GetInt("Progression"));
            defeats = 0;
        }
    }

    public void ResetToLast()
    {
        if (PlayerPrefs.GetInt("Progression") >= checkPointID)
        {
            foreach (EnemyHealth enemy in enemyPool)
            {
                enemy.gameObject.SetActive(false);
            }
        }

        else
        {
            ResetEnemies();
        }
    }

    public void ResetEnemies()
    {
        foreach (EnemyHealth enemy in enemyPool)
        {
            enemy.currentHealth = enemy.health;
            enemy.gameObject.SetActive(true);
            defeats = 0;
        }

        foreach (Enemies.EnemyBehaviourBase enemy in behaviourPool)
        {
            enemy.Reset();
        }

        foreach (GeneralCollectable item in areaCollectables)
        {
            item.gameObject.SetActive(true);
        }
    }
}
