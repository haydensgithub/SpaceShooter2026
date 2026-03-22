using UnityEngine;

public class Game : MonoBehaviour {
  // set in inspector
  public float enemySpawnDelay;
  public GameObject enemyPrefab;
  public GameObject powerupPrefab;
  public BoxCollider2D spawnRange;
  public BoxCollider2D spawnRangeLeft;
  public BoxCollider2D spawnRangeTop;
  public BoxCollider2D spawnRangeBottom;
  public UI ui;

  // private fields
  private float powerUpDelay;
  private float enemySpawnTimer;
  private float powerupSpawnTimer;

  private void Start() {
    powerUpDelay = Random.Range(5f, 10f);
    powerupSpawnTimer = 0;
  }

  private void SpawnEnemy() {
    Vector3 enemySpawnPt = new Vector3(
        Random.Range(spawnRange.bounds.min.x, spawnRange.bounds.max.x),
        Random.Range(spawnRange.bounds.min.y, spawnRange.bounds.max.y),
        0);
    Instantiate(enemyPrefab, enemySpawnPt, Quaternion.identity);
  }

  private void SpawnEnemyLeft() {
    Vector3 leftEnemySpawnPt = new Vector3(
        Random.Range(spawnRangeLeft.bounds.min.x, spawnRangeLeft.bounds.max.x),
        Random.Range(spawnRangeLeft.bounds.min.y, spawnRange.bounds.max.y),
        0);
    Instantiate(enemyPrefab, leftEnemySpawnPt, Quaternion.identity);
  }

  private void SpawnEnemyTop() {
    Vector3 topEnemySpawnPt = new Vector3(
        Random.Range(spawnRangeTop.bounds.min.x, spawnRangeTop.bounds.max.x),
        Random.Range(spawnRangeTop.bounds.min.y, spawnRange.bounds.max.y),
        0);
    Instantiate(enemyPrefab, topEnemySpawnPt, Quaternion.identity);
  }

  private void SpawnEnemyBottom() {
    Vector3 bottomEnemySpawnPt = new Vector3(
        Random.Range(spawnRangeBottom.bounds.min.x, spawnRangeBottom.bounds.max.x),
        Random.Range(spawnRangeBottom.bounds.min.y, spawnRangeBottom.bounds.max.y),
        0);
    Instantiate(enemyPrefab, bottomEnemySpawnPt, Quaternion.identity);
  }
  private void SpawnPowerup() {
    Vector3 powerupSpawnPt = new Vector3(
        Random.Range(spawnRange.bounds.min.x, spawnRange.bounds.max.x),
        Random.Range(spawnRange.bounds.min.y, spawnRange.bounds.max.y),
        0);
    Instantiate(powerupPrefab, powerupSpawnPt, Quaternion.identity);
  }
  void Update() {
    if (!ui.IsReady) {
      return;
    }

    // check spawn enemy
    enemySpawnTimer += Time.deltaTime;
    if (enemySpawnTimer >= enemySpawnDelay) {
      SpawnEnemy();
      SpawnEnemyLeft();
      SpawnEnemyTop();
      SpawnEnemyBottom();
      enemySpawnTimer = 0.0f;
    }

    // check spawn powerup
    powerupSpawnTimer += Time.deltaTime;
    if (powerupSpawnTimer >= powerUpDelay) {
      SpawnPowerup();
      powerUpDelay = Random.Range(5, 10);
      powerupSpawnTimer = 0.0f;
    }
  }
}
