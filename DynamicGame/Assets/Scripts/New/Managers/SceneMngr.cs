using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneMngr : MonoBehaviour
{
    [SerializeField]
    private GameObject menu;
    // Start is called before the first frame update
    void Start()
    {
        menu.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale == 0)
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
        SceneManager.LoadScene("GamePrototype");
        Time.timeScale = 1;
        PlayerPrefs.SetInt("FirstAttempt", 0);
    }
    public void Restart()
    {
        Time.timeScale = 1;
        PlayerPrefs.SetInt("FirstAttempt", 1);
    }
}
