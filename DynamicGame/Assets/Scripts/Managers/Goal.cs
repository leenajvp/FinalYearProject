using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class Goal : MonoBehaviour, Iinteractive
{
    public bool notCompleted { get; set; }
    [SerializeField] GameObject GoalMenu;
    [SerializeField] SceneMngr sceneManager;

    void Start()
    {
        notCompleted = true;
        GoalMenu.SetActive(false);
    }

    public void GetInteraction()
    {
        GoalMenu.SetActive(true);
        sceneManager.DisablePlayer();
    }
}
