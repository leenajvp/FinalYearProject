using UnityEngine;

namespace Enemies
{
    [CreateAssetMenu(fileName = "newEnemy", menuName = "Enemy")]
    public class EnemyData : ScriptableObject
    {
        [Header("Enemy Settings")]
        public string enemyName = "Enemy";
        public int health = 10;
        public GameObject prefab;
        public float walkingSpeed = 3f, runningSpeed = 5f;

        [Header("Enemy Attack Settings")]
        public int bulletDamage = 1;
        public float shootDistance = 8f, lostDistance = 20f, retreatDistance = 5f;
        [Tooltip("Seconds between shots")]
        public float shootSpeed = 1f;

        // Search time?
    }
}
