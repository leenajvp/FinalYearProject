using DDA;
using Enemies;
using UnityEngine;

[RequireComponent(typeof(EnemyBehaviourBase))]
public class EnemyHealth : MonoBehaviour
{
    public float health = 10.0f;
    public float currentHealth = 10.0f;
    DDAManager ddaManager;
    [SerializeField] public bool spawnObject;
    [SerializeField] public GameObject objectToSpawn;
    [SerializeField] private EnemyPools pool;

    public bool explorationNPC = false;

    private void Start()
    {
        if (ddaManager == null)
        {
            ddaManager = FindObjectOfType<DDAManager>();
        }

        if(pool == null)
            pool = transform.parent.GetComponent<EnemyPools>();

        currentHealth = health;
    }

    void Update()
    {
        if (currentHealth <= 0)
        {
            ddaManager.currentKills++;
            pool.UpdateProgress();

            if (spawnObject)
            {
                Instantiate(objectToSpawn, transform.position, objectToSpawn.transform.rotation);
            }

            gameObject.SetActive(false);
        }
    }
}
