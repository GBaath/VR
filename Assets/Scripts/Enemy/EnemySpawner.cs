using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class EnemyChance {
    public Enemy enemy;
    [Tooltip("Chance for this enemy to spawn over other enemies.")]
    [Range(0, 100)] public int chance = 0;
}

public class EnemySpawner : MonoBehaviour {
    public List<EnemyChance> enemyChanceList = new();
    [SerializeField] bool spawnEnemy = false;

    Enemy GetRandomEnemyByChance() {
        int chanceIncrease = 0;
        foreach (EnemyChance enemyChance in enemyChanceList.OrderBy(e => e.chance)) {
            if (enemyChance.chance + chanceIncrease >= Random.Range(1, 100) || enemyChance == enemyChanceList.OrderBy(e => e.chance).Last()) {
                return enemyChance.enemy;
            } else {
                chanceIncrease += enemyChance.chance;
                continue;
            }
        }
        Debug.LogWarning("Something went wrong with enemy spawn chance.", this);
        return enemyChanceList[0].enemy;
    }

    private void Update() {
        if (spawnEnemy) {
            spawnEnemy = false;
            if (enemyChanceList.Count >= 1) {
                SpawnEnemy(GetRandomEnemyByChance());
            } else {
                Debug.LogWarning("Enemy not specified!");
            }
        }
    }

    public void SpawnEnemy(Enemy enemyToSpawn = null) {
        GameObject newEnemy;

        int randomSpawnChance = Random.Range(1, 100) + GameManager.instance.roomManager.roomsPassed * 5;
        int chanceToSpawn = 100 - 25;
        if (randomSpawnChance < chanceToSpawn) { return; }

        if (enemyToSpawn) {
            newEnemy = Instantiate(enemyToSpawn.gameObject, transform.position, Quaternion.identity, transform);
        } else if (enemyChanceList.Count >= 1) {
            newEnemy = Instantiate(GetRandomEnemyByChance().gameObject, transform.position, Quaternion.identity, transform);
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
