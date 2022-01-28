using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneMngr : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject player;
    [SerializeField] private List<EnemyPools> checkPoints = new List<EnemyPools>();

    void Start()
    {
        menu.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.None;

        
    }

    void Update()
    {
        if (Time.timeScale == 0)
        {
            menu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        else
        {
            menu.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void NewGame()
    {
        Time.timeScale = 1;
        PlayerPrefs.SetInt("Progression", 0);

        foreach (EnemyPools checkPoint in checkPoints)
        {

                
                    checkPoint.ResetEnemies();
                
            
        }

    }
    public void Restart()
    {
        Time.timeScale = 1;

        int progression = PlayerPrefs.GetInt("Progression");

        foreach (EnemyPools checkPoint in checkPoints)
        {
            for (int i = 0; i < checkPoints.Count; i++)
            {
                if(checkPoints[i].checkPointID == progression)
                {
                    player.transform.position = checkPoints[i].gameObject.transform.position;
                }

                else
                {
                    checkPoint.ResetEnemies();
                }
            }
        }
        
    }
}
