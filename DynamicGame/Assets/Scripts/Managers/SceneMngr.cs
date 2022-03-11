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
    [SerializeField] public Transform startpos;
    [SerializeField] private List<EnemyPools> checkPoints = new List<EnemyPools>();

    private GameObject player;
    private PlayerHealth pHealth;
    private Player.PlayerController pController;

    void Start()
    {
        PlayerPrefs.SetInt("Progression", 0);
        pController = FindObjectOfType<PlayerController>();
        pHealth = FindObjectOfType<PlayerHealth>();
        player = pHealth.gameObject;

        newGameMenu.SetActive(true);
        gameOver.SetActive(false);
        DisablePlayer();
    }

    void Update()
    {
        if (pHealth.currentHealth <= 0)
        {
            DisablePlayer();
            gameOver.SetActive(true);
        }
    }

    public void NewGame()
    {
        DisablePlayer();
        Time.timeScale = 1;
        newGameMenu.SetActive(false);
        PlayerPrefs.SetInt("Progression", 0);
        player.transform.position = startpos.position;

        foreach (EnemyPools checkPoint in checkPoints)
        {
            checkPoint.ResetEnemies();
        }

    }
    public void Restart()
    {
        DisablePlayer();
        Time.timeScale = 1;
        gameOver.SetActive(false);

        int progression = PlayerPrefs.GetInt("Progression");

        if (PlayerPrefs.GetInt("Progression") == 0)
        {
            player.transform.position = startpos.position;
            return;
        }

        foreach (EnemyPools checkPoint in checkPoints)
        {
            for (int i = 0; i < checkPoints.Count; i++)
            {
                if (checkPoints[i].checkPointID == progression && PlayerPrefs.GetInt("ExplorationSave") == 0)
                {
                    player.transform.position = checkPoints[i].gameObject.transform.position;
                }

                checkPoint.ResetToLast();
            }
        }
    }

    private void DisablePlayer()
    {
        pController.PauseGame();
        pController.DisablePlayer();
        pController.DisableMenus();
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
