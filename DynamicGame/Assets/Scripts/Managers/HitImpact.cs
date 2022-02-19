using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitImpact : MonoBehaviour
{
    [SerializeField] private BulletData bulletData;
    private ObjectPool pool;

    public Vector3 target { get; set; }
    public bool hit { get; set; }

    protected virtual void Start()
    {
        pool = transform.parent.GetComponent<ObjectPool>();
    }

    private void OnEnable()
    {
        StartCoroutine(DestroyTimer());
    }

    private void Update()
    {
        if (!hit && Vector3.Distance(transform.position, target) < .01f)
        {
            pool.ReturnObject(gameObject);
        }
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        pool.ReturnObject(gameObject);
    }


    private IEnumerator DestroyTimer()
    {
        yield return new WaitForSeconds(bulletData.timeToDestroy);
        pool.ReturnObject(gameObject);
    }
}
