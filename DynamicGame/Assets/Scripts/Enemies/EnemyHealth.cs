using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using DDA;
#if UNITY_EDITOR
using UnityEditor;
#endif

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

    // Update is called once per frame
    void Update()
    {
        if(currentHealth <= 0)
        {
            ddaManager.currentKills++;

            if (spawnObject)
            {
                // -- BUG -- object collection on instantiate
                //Instantiate(objectToSpawn, transform.position, objectToSpawn.transform.rotation);
                // Temp solution
                objectToSpawn.transform.position = transform.position;
                objectToSpawn.SetActive(true);
                gameObject.SetActive(false);
            }

            else
            gameObject.SetActive(false);
        }
    }

    //Custom editor for obejct spawn on death
//#if UNITY_EDITOR
//    [CustomEditor(typeof(EnemyHealth))]
//    public class EnemyHealthEditor : Editor
//    {
//        public override void OnInspectorGUI()
//        {
//            DrawDefaultInspector();

//            EnemyHealth script = (EnemyHealth)target;

//            script.spawnObject = EditorGUILayout.Toggle("Spawn Object", script.spawnObject);
//            if (script.spawnObject)
//            {
//                script.objectToSpawn = EditorGUILayout.ObjectField("Object to Spawn", script.objectToSpawn, typeof(GameObject), true) as GameObject;
//            }
//        }
//    }
//#endif
}
