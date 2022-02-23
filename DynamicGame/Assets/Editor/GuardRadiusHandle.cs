using UnityEditor;
using UnityEngine;
using Enemies;

[CustomEditor(typeof(EnemyBehaviourBase))]
public class GuardRadiusHandle : Editor
{
    private void OnSceneGUI()
    {
        Handles.color = Color.red;
        EnemyBehaviourBase enemy = (EnemyBehaviourBase)target;
        Handles.DrawWireArc(enemy.transform.position, enemy.transform.up, -enemy.transform.right, 360, enemy.detectionRadius);
    }
}
