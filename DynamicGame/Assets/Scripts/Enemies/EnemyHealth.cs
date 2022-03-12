using DDA;
using Enemies;
using UnityEngine;

[RequireComponent(typeof(EnemyBehaviourBase))]
public class EnemyHealth : MonoBehaviour
{
    [Tooltip("Enemydata to inherit")]
    [SerializeField]private  EnemyData data;
    [Tooltip("Health is managed by data")]
    public float currentHealth;
    public DDAManager ddaManager;

    [Header("Object spawning on defeat")]
    [Tooltip("Set true if object is spawned")]
    [SerializeField] public bool spawnObject;
    [SerializeField] public GameObject objectToSpawn;
    [SerializeField] private EnemyPools pool;
    [HideInInspector] public float health;

    public bool explorationNPC = false;

    private void Start()
    {
        if (ddaManager == null)
            ddaManager = FindObjectOfType<DDAManager>();

        currentHealth = data.health;
    }

    void Update()
    {
        health = data.health;

        if (data.adjust)
            currentHealth = health;

        if (currentHealth <= 0)
        {
            if (pool != null)
                pool.UpdateProgress();

            if (spawnObject)
            {
                Instantiate(objectToSpawn, transform.position, objectToSpawn.transform.rotation);
                spawnObject = false;
            }

            ddaManager.currentKills++;
            gameObject.SetActive(false);
        }
    }
}
