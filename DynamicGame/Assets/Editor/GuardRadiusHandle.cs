using Enemies;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyStates))]
public class GuardRadiusHandle : Editor
{
    private void OnSceneGUI()
    {
        Handles.color = Color.red;
        EnemyStates guard = (EnemyStates)target;
        Handles.DrawWireArc(guard.transform.position, guard.transform.up, -guard.transform.right, 360, guard.detectionRadius);
    }
}
