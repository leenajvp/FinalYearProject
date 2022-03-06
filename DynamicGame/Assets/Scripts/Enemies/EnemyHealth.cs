using DDA;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health = 10.0f;
    public float currentHealth = 10.0f;
    DDAManager ddaManager;
    [SerializeField] public bool spawnObject;
    [SerializeField] public GameObject objectToSpawn;

    private void Start()
    {
        if (ddaManager == null)
        {
            ddaManager = FindObjectOfType<DDAManager>();
        }

        currentHealth = health;

    }

    void Update()
    {
        if (currentHealth <= 0)
        {
            ddaManager.currentKills++;

            if (spawnObject)
            {
                Instantiate(objectToSpawn, transform.position, objectToSpawn.transform.rotation);
                gameObject.SetActive(false);
            }

            else
                gameObject.SetActive(false);
        }
    }
}
