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
        Handles.DrawWireArc(enemy.transform.position, enemy.transform.up, -enemy.transform.right, 360, enemy.detectionRadius);
    }
}

[CustomEditor(typeof(StillEnemy))]
public class StillEnemy : Editor
{
    //private void OnSceneGUI()
    //{
    //    Handles.color = Color.red;
    //    StillEnemy enemy = (StillEnemy)target;
    //    Handles.DrawWireArc(enemy.transform.position, enemy.transform.up, -enemy.transform.right, 360, enemy.detectionRadius);
    //}
}


