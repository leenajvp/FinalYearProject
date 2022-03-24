using System.Collections.Generic;
using UnityEngine;
using Player;
using DDA;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SceneMngr : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject newGameMenu;
    [SerializeField] public Transform startpos;
    [SerializeField] private List<EnemyPools> checkPoints = new List<EnemyPools>();
    private PlayerHealth pHealth;
    private PlayerController pController;
    [SerializeField] DDAManager dda;
    [SerializeField] private GameObject LockUI;

    void Start()
    {

        PlayerPrefs.SetInt("Progression", 0);
        PlayerPrefs.SetInt("ExplorationSave", 0);

        if (player == null)
            player = FindObjectOfType<PlayerController>().gameObject;

        pHealth = player.GetComponent<PlayerHealth>();
        pController = player.GetComponent<PlayerController>();

        newGameMenu.SetActive(true);
        gameOver.SetActive(false);
        DisablePlayer();
    }

    void Update()
    {
        if (dda.playerDead)
        {
            DisablePlayer();
            gameOver.SetActive(true);
        }
    }

    public void NewGame()
    {
        gameObject.SetActive(true);
        DisablePlayer();
        Time.timeScale = 1;
        newGameMenu.SetActive(false);
        PlayerPrefs.SetInt("Progression", 0);
        player.transform.position = startpos.position;

        foreach (EnemyPools checkPoint in checkPoints)
        {
            checkPoint.ResetEnemies();
        }

        for (int i = 0; i < LockUI.transform.childCount; i++)
        {
            var child = LockUI.transform.GetChild(i);
            if (child.gameObject.activeSelf)
                child.gameObject.SetActive(false);
        }

    }
    public void Restart()
    {
        player.SetActive(true);
        DisablePlayer();
        Time.timeScale = 1;
        gameOver.SetActive(false);

        int progression = PlayerPrefs.GetInt("Progression");

        if (PlayerPrefs.GetInt("Progression") == 0)
        {
            player.transform.position = startpos.position;
        }

        foreach (EnemyPools checkPoint in checkPoints)
        {
            checkPoint.ResetToLast();

            for (int i = 0; i < checkPoints.Count; i++)
            {
                if (checkPoints[i].checkPointID == progression)
                {
                    player.transform.position = checkPoints[i].gameObject.transform.position;
                    
                }
            }
        }

        for (int i = 0; i < LockUI.transform.childCount; i++)
        {
            var child = LockUI.transform.GetChild(i);
            if (child.gameObject.activeSelf)
                child.gameObject.SetActive(false);
        }
    }

    public void DisablePlayer()
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
