using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyData))]
public class EnemyDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EnemyData enemyData = (EnemyData)target;
        if (GUILayout.Button("Refresh Enemy Data"))
        {
            enemyData.RefreshEnemyData();
        }
    }
}
