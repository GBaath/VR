using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemy;
    [SerializeField] bool spawnEnemy = false;

    private void Update()
    {
        if (spawnEnemy)
        {
            spawnEnemy = false;
            if (enemy) { SpawnEnemy(enemy); }
            else { Debug.LogError("Enemy not specified! {1}"); }
        }
    }

    public void SpawnEnemy(GameObject enemy = null)
    {
        GameObject newEnemy;

        if (!this.enemy && !enemy) { Debug.LogError("Enemy not specified! {2}"); return; }
        else if (enemy != null) { newEnemy = Instantiate(enemy, transform); }
        else if (this.enemy) { newEnemy = Instantiate(this.enemy, transform); }
    }
}
