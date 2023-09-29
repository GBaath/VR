using System.Linq;
using UnityEditor;

[CustomEditor(typeof(EnemySpawner))]
public class EnemySpawnerEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        EnemySpawner enemySpawner = (EnemySpawner)target;
        int totalChanceRemaining = 100;
        foreach (EnemyChance enemyChance in enemySpawner.enemyChanceList.OrderBy(e => e.chance)) {
            if (totalChanceRemaining > enemyChance.chance) {
                totalChanceRemaining -= enemyChance.chance;
            } else {
                enemyChance.chance = totalChanceRemaining;
                totalChanceRemaining = 0;
            }
        }
    }
}
