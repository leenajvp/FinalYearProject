using System.Collections.Generic;
using UnityEngine;
using Player;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SceneMngr : MonoBehaviour
{
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject newGameMenu;
    [SerializeField] private List<EnemyPools> checkPoints = new List<EnemyPools>();

    private GameObject player;
    private PlayerHealth pHealth;
    private PlayerController pController;

    void Start()
    {
        pController = FindObjectOfType<PlayerController>();
        pHealth = FindObjectOfType<PlayerHealth>();
        player = pHealth.gameObject;

        pController.PauseGame();
        newGameMenu.SetActive(true);
        gameOver.SetActive(false);
    }

    void Update()
    {
        if (pHealth.currentHealth <= 0)
        {
            pController.PauseGame();
            gameOver.SetActive(true);
        }
    }

    public void NewGame()
    {
        pController.PauseGame();
        Time.timeScale = 1;
        newGameMenu.SetActive(false);
        PlayerPrefs.SetInt("Progression", 0);

        foreach (EnemyPools checkPoint in checkPoints)
        {
            checkPoint.ResetEnemies();
        }

    }
    public void Restart()
    {
        pController.PauseGame();
        Time.timeScale = 1;
        gameOver.SetActive(false);

        int progression = PlayerPrefs.GetInt("Progression");

        foreach (EnemyPools checkPoint in checkPoints)
        {
            for (int i = 0; i < checkPoints.Count; i++)
            {
                if (checkPoints[i].checkPointID == progression)
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

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
