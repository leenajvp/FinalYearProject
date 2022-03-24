using UnityEngine;
using UnityEngine.UI;

namespace Enemies
{
    [CreateAssetMenu(fileName = "newEnemy", menuName = "Enemy")]
    public class EnemyData : ScriptableObject
    {
        [Header("Enemy Settings")]
        public string enemyName = "Enemy";
        public int health = 10;
        public float walkingSpeed = 3f, runningSpeed = 5f;
        public float returnGuardDistance= 1f;

        [Header("Materials")]
        public Material defaultMaterial;
        public Material hurtMaterial;

        [Header("Sounds")]
        public AudioClip hitSound;

        [Header("Visualisation")]
        public Image pHitEffect; // dont want this left here 

        [Header("Player Detection")]
        [Tooltip("The guard will detect if player enters this radius and begin raycast.")]
        public float detectionRadius = 20f;
        public float detectionRayDistance = 20f;
        public float rotationSpeed = 5f;

        [Header("Enemy Attack Settings")]
        public int bulletDamage = 1;
        public float shootDistance = 8f, lostDistance = 20f, retreatDistance = 5f;
        [Tooltip("Seconds between shots")]
        public float shootSpeed = 1f;

        public bool adjust = false;
    }
}
