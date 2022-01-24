using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bullets {
    public class BulletController : MonoBehaviour
    {
        [SerializeField] private float speed = 50.0f;
        private float timeToDestroy = 5.0f;

        public Vector3 target { get; set; }
        public bool hit { get; set; }

        private void OnEnable()
        {
            Destroy(gameObject, timeToDestroy);
        }

        private void Update()
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

            if(!hit && Vector3.Distance(transform.position, target) < .01f)
            {
                Destroy(gameObject);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            Destroy(gameObject);
        }

    }
}
