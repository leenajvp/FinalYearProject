using System.Collections.Generic;
using UnityEngine;

public class EnemyPools : MonoBehaviour
{
    [SerializeField] public List<EnemyHealth> enemyPool = new List<EnemyHealth>();
    public int checkPointID = 0;
    public int defeats = 0;
    public int enemies;
    public bool completed = false;
    private bool isDefeated;
    private DDA.DDAManager ddaManager;

    private void Start()
    {
        //enemies = enemyPool.Count;
        enemies = enemyPool.Count;
    }

    void Update()
    {
        
    }

    public void UpdateProgress()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
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
            Debug.Log(PlayerPrefs.GetInt("Progression"));

        }
    }

    public void ResetToLast()
    {
        if (PlayerPrefs.GetInt("Progression") >= checkPointID)
        {
            foreach (EnemyHealth enemy in enemyPool)
            {
                enemy.gameObject.SetActive(false);
                return;
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
            isDefeated = false;
            enemy.gameObject.SetActive(true);
        }
    }


}
