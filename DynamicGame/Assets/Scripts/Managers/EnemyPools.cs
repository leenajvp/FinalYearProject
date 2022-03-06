using System.Collections;
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

    private void Start()
    {
        //enemies = enemyPool.Count;
        enemies = transform.childCount;
    }

    void Update()
    {
        UpdateProgress();
    }

    private void UpdateProgress()
    {
        for(int i = 0; i < enemyPool.Count - 1; i++)
        {
            if(enemyPool[i].currentHealth <= 0)
            {
                defeats++;

                if(defeats >= enemies)
                {
                    PlayerPrefs.SetInt("Progression", checkPointID);
                    Debug.Log(PlayerPrefs.GetInt("Progression"));
                }
            }
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
            isDefeated=false;
            enemy.gameObject.SetActive(true);
        }
    }


}
