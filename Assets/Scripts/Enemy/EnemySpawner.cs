using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    [SerializeField] List<Enemy> enemies = new();
    [SerializeField] bool spawnEnemy = false;

    private void Update() {
        if (spawnEnemy) {
            spawnEnemy = false;
            if (enemies.Count >= 1) {
                int randomEnemyID = Random.Range(0, enemies.Count);
                SpawnEnemy(enemies[randomEnemyID]);
            } else {
                Debug.LogError("Enemy not specified!");
            }
        }
    }

    public void SpawnEnemy(Enemy enemy = null) {
        GameObject newEnemy;
        if (enemy) {
            newEnemy = Instantiate(enemy.gameObject, transform.position, Quaternion.identity, transform);
        } else if (enemies.Count >= 1) {
            newEnemy = Instantiate(enemies[Random.Range(0, enemies.Count)].gameObject, transform.position, Quaternion.identity, transform);
        } else {
            Debug.LogError("Enemy not specified!");
            return;
        }
    }
}
