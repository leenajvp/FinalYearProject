using Enemies;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyBehaviour))]
public class GuardRadiusHandle : Editor
{
    private void OnSceneGUI()
    {
        Handles.color = Color.red;
        EnemyBehaviour guard = (EnemyBehaviour)target;
        Handles.DrawWireArc(guard.transform.position, guard.transform.up, -guard.transform.right, 360, guard.detectionRadius);
    }
}
