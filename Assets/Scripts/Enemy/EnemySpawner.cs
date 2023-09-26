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
        if (newEnemy.TryGetComponent(out Enemy enemyScript)) {
            //enemyScript.wasSpawned = true;
            RandomizeSizeAndStats(enemyScript);
        } else {
            Debug.LogWarning("Enemy has no Enemy script!");
            return;
        }
    }

    void RandomizeSizeAndStats(Enemy enemy) {
        float maxScale = 1.8f;
        float minScale = 0.2f;
        float randomScaleFloat = Random.Range(minScale, maxScale);
        int scaleInt = 0;
        for (int i = 0; i < randomScaleFloat / minScale; i++) {
            scaleInt++;
        }
        enemy.transform.localScale = new Vector3(enemy.EnemyData.StartSize * randomScaleFloat, enemy.EnemyData.StartSize * randomScaleFloat, enemy.EnemyData.StartSize * randomScaleFloat);
        //enemy.maxHealth = enemy.EnemyData.MaxHealth + scaleInt;
        //if (enemy.TryGetComponent(out HealthProperty hp)) {

        //}
        //enemy.turnSpeed = enemy.EnemyData.TurnSpeed * randomScaleFloat;
        //enemy.attackDamage = (int)(enemy.EnemyData.AttackDamage * randomScaleFloat);
        //enemy.movementSpeed = enemy.EnemyData.MovementSpeed * randomScaleFloat;
    }
}
