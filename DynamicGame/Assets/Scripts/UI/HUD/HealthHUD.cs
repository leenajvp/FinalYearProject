using Player;
using UnityEngine;
using UnityEngine.UI;

public class HealthHUD : MonoBehaviour
{
    [SerializeField] private Image healthBarFill;
    private PlayerHealth health;
    private Slider hSlider;

    void Start()
    {
        health = FindObjectOfType<PlayerHealth>();
        hSlider = GetComponent<Slider>();
    }

    void Update()
    {
        hSlider.value = health.currentHealth;

        if (health.currentHealth < 7)
            healthBarFill.color = Color.red;

        else if(health.currentHealth >= 7 && health.currentHealth < 12)
            healthBarFill.color = Color.yellow;

        else
            healthBarFill.color = Color.green;
    }
}
