using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bullets {
    public class BulletController : MonoBehaviour
    {
        [SerializeField] private float speed = 50.0f;
        private float timeToDestroy = 5.0f;
        public ObjectPool pool;

        public Vector3 target { get; set; }
        public bool hit { get; set; }

        private void OnEnable()
        {
            StartCoroutine(DestroyBulletTimer());
        }

        protected virtual void Start()
        {
            pool = transform.parent.GetComponent<ObjectPool>();
        }

        private void Update()
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

            if(!hit && Vector3.Distance(transform.position, target) < .01f)
            {
                pool.ReturnObject(gameObject);
            }
        }

        protected virtual void OnCollisionEnter(Collision collision)
        {
            pool.ReturnObject(gameObject);
        }

        private IEnumerator DestroyBulletTimer()
        {
            yield return new WaitForSeconds(3);
            pool.ReturnObject(gameObject);
        }

    }
}
