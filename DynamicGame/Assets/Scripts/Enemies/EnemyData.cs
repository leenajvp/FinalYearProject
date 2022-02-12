using UnityEngine;

namespace Enemies
{
    [CreateAssetMenu(fileName = "newEnemy", menuName = "Enemy")]
    public class EnemyData : ScriptableObject
    {
        [Header("Enemy Settings")]
        public string enemyName;
        public int health;
        public GameObject prefab;
        public float walkingSpeed, runningSpeed;

        [Header("Enemy Attack Settings")]
        public int bulletDamage;
        public float shootDistance, lostDistance;
        public float retreatDistance;
    }
}
