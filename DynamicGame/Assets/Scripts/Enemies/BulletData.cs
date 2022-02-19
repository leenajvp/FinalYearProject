using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Bullet", menuName = "Bullet")]
public class BulletData : ScriptableObject
{
    [Header("Bullet Behaviour")]
    public float speed = 50.0f;

    [Tooltip("Bullet's living time")]
    public float timeToDestroy = 3.0f;

    // Add some effects when hit and shot
}
