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
                Debug.LogWarning("Enemy not specified!");
            }
        }
    }

    public void SpawnEnemy(Enemy enemyToSpawn = null) {
        GameObject newEnemy;
        if (enemyToSpawn) {
            newEnemy = Instantiate(enemyToSpawn.gameObject, transform.position, Quaternion.identity, transform);
        } else if (enemies.Count >= 1) {
            newEnemy = Instantiate(enemies[Random.Range(0, enemies.Count)].gameObject, transform.position, Quaternion.identity, transform);
        } else {
            Debug.LogWarning("Enemy not specified!");
            return;
        }
        if (!newEnemy.TryGetComponent(out Enemy enemy)) {
            Debug.LogWarning("Enemy script missing!");
            return;
        }
        enemy.movementSpeed += GameManager.instance.roomManager.roomsPassed / 10;
        if (TryGetComponent(out HealthProperty hp)) { hp.maxHealth += GameManager.instance.roomManager.roomsPassed / 10; }
    }
}
