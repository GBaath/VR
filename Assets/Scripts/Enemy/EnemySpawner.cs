using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    [SerializeField] GameObject enemy;
    [SerializeField] bool spawnEnemy = false;

    private void Update() {
        if (spawnEnemy) {
            spawnEnemy = false;
            if (enemy) {
                SpawnEnemy(enemy);
            } else {
                Debug.LogError("Enemy not specified!");
            }
        }
    }

    public void SpawnEnemy(GameObject enemy = null) {
        GameObject newEnemy;
        if (enemy) {
            newEnemy = Instantiate(enemy, transform.position, Quaternion.identity, transform);
        } else if (this.enemy) {
            newEnemy = Instantiate(this.enemy, transform.position, Quaternion.identity, transform);
        } else {
            Debug.LogError("Enemy not specified!");
            return;
        }
    }
}
