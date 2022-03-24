using DDA;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPools : MonoBehaviour
{
    [SerializeField] public List<EnemyHealth> enemyPool = new List<EnemyHealth>();
    [SerializeField] public List<GeneralCollectable> areaCollectables = new List<GeneralCollectable>();
    [SerializeField] private DDAManager ddaManager;
    [SerializeField] private List<EnemyHealth> extraEnemies = new List<EnemyHealth>();
    [SerializeField] private List<EnemyHealth> extraEnemies2 = new List<EnemyHealth>();
    public int checkPointID = 0;
    public int defeats = 0;
    public int enemies;
    public bool completed = false;

    public bool stage1 = false;
    public bool stage2 = false;

    public bool stage0 = false;

    public int originalCount;

    private bool check;
    private bool check2;

    // private bool isDefeated;

    private void Start()
    {
        enemies = enemyPool.Count;
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
    }

   

    private void Update()
    {
        if (stage0 == true)
        {
            for (int i = originalCount; i < enemyPool.Count; i++)
            {
                enemyPool[i].gameObject.SetActive(false);
                enemyPool.RemoveAt(i);
                i--;
            }

          //  enemyPool.RemoveAll(enemy => !enemy.gameObject.activeSelf);

            enemies = enemyPool.Count;
            stage0 = false;
        }

        if (stage1 == true)
        {
            for (int i = 0; i < extraEnemies.Count; i++)
            {
                extraEnemies[i].gameObject.SetActive(true);
                enemyPool.Add(extraEnemies[i]);
            }
            enemies = enemyPool.Count;
            stage1 = false;
        }

        if (stage2 == true)
        {
            for (int i = 0; i < extraEnemies2.Count; i++)
            {
                extraEnemies2[i].gameObject.SetActive(true);
                enemyPool.Add(extraEnemies2[i]);
            }
            enemies = enemyPool.Count;
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

        if (defeats >= enemies && PlayerPrefs.GetInt("Progression") < checkPointID)
        {
            PlayerPrefs.SetInt("Progression", checkPointID);
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
            //   isDefeated = false;
            enemy.gameObject.SetActive(true);
            defeats = 0;
        }

        foreach (GeneralCollectable item in areaCollectables)
        {
            item.gameObject.SetActive(true);
        }
    }
}
