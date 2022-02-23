using Player;
using UnityEngine;
using UnityEngine.UI;

public class HealthHUD : MonoBehaviour
{
    private Text healthTxt;
    private PlayerHealth health;

    void Start()
    {
        health = FindObjectOfType<PlayerHealth>();
        healthTxt = GetComponent<Text>();
    }

    void Update()
    {
        healthTxt.text = health.currentHealth.ToString();
    }
}
