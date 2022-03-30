using DDA;
using Enemies;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(EnemyBehaviourBase), typeof (AudioSource))]
public class EnemyHealth : MonoBehaviour
{
    [Tooltip("Enemydata to inherit")]
    [SerializeField] private EnemyData data;
    [Tooltip("Health is managed by data")]
    public float currentHealth;
    [SerializeField] public EnemyPools pool;
    public DDAManager ddaManager;

    [Header("Object spawning on defeat")]
    [Tooltip("Set true if object is spawned")]
    [SerializeField] public bool spawnObject;
    [SerializeField] public GameObject objectToSpawn;
    [HideInInspector] public float health;

    private AudioSource hitSound;
    private Renderer ren;
    private Material defaultMaterial, hurtMaterial;

    public bool explorationNPC = false;

    private void Start()
    {
        hitSound = GetComponent<AudioSource>();
        hitSound.clip = data.hitSound;
        ren = GetComponent<Renderer>();
        defaultMaterial = data.defaultMaterial;
        hurtMaterial = data.hurtMaterial;
        currentHealth = data.health;
        ren.material = data.defaultMaterial;

        if (ddaManager == null)
            ddaManager = FindObjectOfType<DDAManager>();

        
    }

    void Update()
    {
        health = data.health;

        if (currentHealth <= 0)
        {
            if (pool != null)
              pool.UpdateProgress();


            if (spawnObject)
            {
                Instantiate(objectToSpawn, transform.position, objectToSpawn.transform.rotation);
            }

            ddaManager.currentKills++;
            gameObject.SetActive(false);
        }
    }

    public void ChangeMaterial()
    {
        ren.material = hurtMaterial;
        hitSound.PlayDelayed(0.02f);
        StartCoroutine(ReactTimer());
    }

    private IEnumerator ReactTimer()
    {
        yield return new WaitForSeconds(0.2f);
        ren.material = defaultMaterial;
    }
}
