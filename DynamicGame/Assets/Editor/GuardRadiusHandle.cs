using UnityEditor;
using UnityEngine;
using Enemies;

[CustomEditor(typeof(PatrollingEnemyBehaviour))]
public class EnemyRadiusHandle : Editor
{
    private void OnSceneGUI()
    {
        Handles.color = Color.red;
        PatrollingEnemyBehaviour enemy = (PatrollingEnemyBehaviour)target;
        Handles.DrawWireArc(enemy.transform.position, enemy.transform.up, -enemy.transform.right, 360, enemy.data.detectionRadius);
    }
}

[CustomEditor(typeof(GuardingEnemy))]
public class GuardRadiusHandle : Editor
{
    private void OnSceneGUI()
    {
        Handles.color = Color.red;
        GuardingEnemy guard = (GuardingEnemy)target;
        Handles.DrawWireArc(guard.transform.position, guard.transform.up, -guard.transform.right, 360, guard.data.detectionRadius);
    }
}


