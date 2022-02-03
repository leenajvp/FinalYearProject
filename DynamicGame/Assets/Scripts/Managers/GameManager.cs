using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool playerDetected;
    [SerializeField] private List<CheckPoints> enemyPool1 = new List<CheckPoints>();


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {

        //foreach(EnemyHealth enemy in enemyPool1)
        //{
        //    if(enemy.health <= 0)
        //    {
        //        enemyPool1.Remove(enemy);
        //    }
        //}
    }


}
