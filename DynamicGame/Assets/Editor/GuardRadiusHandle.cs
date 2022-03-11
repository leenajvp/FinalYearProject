using UnityEditor;
using UnityEngine;
using Enemies;

[CustomEditor(typeof(PatrollingEnemyBehaviour))]
public class GuardRadiusHandle : Editor
{
    private void OnSceneGUI()
    {
        Handles.color = Color.red;
        PatrollingEnemyBehaviour enemy = (PatrollingEnemyBehaviour)target;
        Handles.DrawWireArc(enemy.transform.position, enemy.transform.up, -enemy.transform.right, 360, enemy.data.detectionRadius);
    }
}

//[CustomEditor(typeof(GuardingEnemy))]
//public class GuardingEnemy : Editor
//{
//    private void OnSceneGUI()
//    {
//        Handles.color = Color.red;
//        GuardingEnemy enemy = (GuardingEnemy)target;
//        Handles.DrawWireArc(enemy.transform.position, enemy.transform.up, -enemy.transform.right, 360, enemy.detectionRadius);
//    }
//}


