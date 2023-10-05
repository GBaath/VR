using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class EntityChance {
    public GameObject entity;
    [Tooltip("Chance for this enemy to spawn over other enemies.")]
    [Range(0, 100)] public int chance = 0;
}

public class EntitySpawner : MonoBehaviour {
    public List<EntityChance> entityChanceList = new();
    [SerializeField] bool spawnEntity = false;

    GameObject GetRandomEnemyByChance() {
        int chanceIncrease = 0;
        foreach (EntityChance entityChance in entityChanceList.OrderBy(e => e.chance)) {
            if (entityChance.chance + chanceIncrease >= Random.Range(1, 100) || entityChance == entityChanceList.OrderBy(e => e.chance).Last()) {
                return entityChance.entity;
            } else {
                chanceIncrease += entityChance.chance;
                continue;
            }
        }
        Debug.LogWarning("Something went wrong with enemy spawn chance.", this);
        return entityChanceList[0].entity;
    }

    private void Update() {
        if (spawnEntity) {
            spawnEntity = false;
            if (entityChanceList.Count >= 1) {
                SpawnEntity(GetRandomEnemyByChance());
            } else {
                Debug.LogWarning("Enemy not specified!");
            }
        }
    }

    public void SpawnEntity(GameObject spawnEntity = null) {
        GameObject newEntity;

        int randomSpawnChance = Random.Range(1, 100) + GameManager.instance.roomManager.roomsPassed * 5;
        int chanceToSpawn = 100 - 50;
        if (randomSpawnChance < chanceToSpawn) { return; }

        if (spawnEntity) {
            newEntity = Instantiate(spawnEntity, transform.position, Quaternion.identity, transform);
        } else if (entityChanceList.Count >= 1) {
            newEntity = Instantiate(GetRandomEnemyByChance(), transform.position, Quaternion.identity, transform);
        } else {
            Debug.LogWarning("Enemy not specified!");
            return;
        }
        if (newEntity.TryGetComponent(out Enemy enemy)) {
            enemy.movementSpeed += GameManager.instance.roomManager.roomsPassed * 10;
        }
        if (TryGetComponent(out HealthProperty hp)) {
            hp.Health += GameManager.instance.roomManager.roomsPassed * 10;
        }
    }
}
