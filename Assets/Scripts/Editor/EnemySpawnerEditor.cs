using System.Linq;
using UnityEditor;

[CustomEditor(typeof(EntitySpawner))]
public class EnemySpawnerEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        EntitySpawner enemySpawner = (EntitySpawner)target;
        int totalChanceRemaining = 100;
        foreach (EntityChance enemyChance in enemySpawner.entityChanceList.OrderBy(e => e.chance)) {
            if (totalChanceRemaining > enemyChance.chance) {
                totalChanceRemaining -= enemyChance.chance;
            } else {
                enemyChance.chance = totalChanceRemaining;
                totalChanceRemaining = 0;
            }
        }
    }
}
