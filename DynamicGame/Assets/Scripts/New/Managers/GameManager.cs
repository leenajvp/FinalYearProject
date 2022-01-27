using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool playerDetected;
    [SerializeField]
    private List<EnemyHealth> enemyPool1 = new List<EnemyHealth>();
    private List<GameObject> enemyPool2 = new List<GameObject>();
    private List<GameObject> enemyPool3 = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        ManageEnemyNumbers();

        foreach(EnemyHealth enemy in enemyPool1)
        {
            if(enemy.health <= 0)
            {
                enemyPool1.Remove(enemy);
            }
        }
    }

    private void ManageEnemyNumbers()
    {
        if (enemyPool1.Count==0)
        {
            PlayerPrefs.SetInt("Progression", 1);
        }

        if (enemyPool2.Count == 0)
        {
            PlayerPrefs.SetInt("Progression", 2);
        }

        if (enemyPool3.Count == 0)
        {
            PlayerPrefs.SetInt("Progression", 3);
        }

    }
}
