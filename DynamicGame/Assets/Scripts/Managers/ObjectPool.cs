using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int poolSize;
    [SerializeField] private bool expandable;
    private List<GameObject> availableObjects;
     private List<GameObject> usedObjects;

    private void Start()
    {
        availableObjects = new List<GameObject>();
        usedObjects = new List<GameObject>();

        for(int i = 0; i < poolSize; i++)
        {
            GenerateObject();
        }
    }

    public GameObject GetObject()
    {
        int totalAvailable = availableObjects.Count;

        if (availableObjects.Count == 0 && !expandable)
            return null;

        else if (availableObjects.Count == 0)
            GenerateObject();

        GameObject g = availableObjects[totalAvailable - 1];
        availableObjects.RemoveAt(totalAvailable - 1);
        usedObjects.Add(g);
        return g;
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        usedObjects.Remove(obj);
        availableObjects.Add(obj);
    }

    private void GenerateObject()
    {
        GameObject g = Instantiate(prefab);
        g.transform.parent = transform;
        g.SetActive(false);
        availableObjects.Add(g);
    }
}
