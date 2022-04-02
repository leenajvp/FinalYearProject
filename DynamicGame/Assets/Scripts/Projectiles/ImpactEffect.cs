using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactEffect : MonoBehaviour
{
    private ObjectPool pool;

    private void OnEnable()
    {
        
        StartCoroutine(LiveTimer());
    }

    private void Start()
    {
        pool = transform.parent.GetComponent<ObjectPool>();
    }

    private IEnumerator LiveTimer()
    {
        yield return new WaitForSeconds(0.2f);
        pool.ReturnObject(gameObject);
    }
}

