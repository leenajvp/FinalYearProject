using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DDA;

namespace Bullets {
    public class BulletController : MonoBehaviour
    {
        [SerializeField] private BulletData bulletData;
        protected ObjectPool pool;

        public Vector3 target { get; set; }
        public bool hit { get; set; }

        protected virtual void Start()
        {
            pool = transform.parent.GetComponent<ObjectPool>();
            StartCoroutine(DestroyTimer());
        }

        private void Update()
        {
            transform.position = Vector3.MoveTowards(transform.position, target, bulletData.speed * Time.deltaTime);

            if(!hit && Vector3.Distance(transform.position, target) < .03f)
            {
                pool.ReturnObject(gameObject);
            }
        }

        protected IEnumerator DestroyTimer()
        {
            yield return new WaitForSeconds(2);
            pool.ReturnObject(gameObject);
        }
    }
}
