using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bullets {
    public class BulletController : MonoBehaviour
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
            transform.position = Vector3.MoveTowards(transform.position, target, bulletData.speed * Time.deltaTime);

            if(!hit && Vector3.Distance(transform.position, target) < .01f)
            {
                pool.ReturnObject(gameObject);
            }
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            pool.ReturnObject(gameObject);
        }


        private IEnumerator DestroyTimer()
        {
            yield return new WaitForSeconds(bulletData.timeToDestroy);
            pool.ReturnObject(gameObject);
        }
    }
}
