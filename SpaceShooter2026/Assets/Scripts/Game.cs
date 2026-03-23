using UnityEngine;

public class Game : MonoBehaviour {
  // set in inspector
  public GameObject enemyPrefab;
  public GameObject powerupPrefab;
  public BoxCollider2D spawnRange;
  public BoxCollider2D spawnRangeLeft;
  public BoxCollider2D spawnRangeTop;
  public BoxCollider2D spawnRangeBottom;
  public UI ui;

  // Spawn delay ranges
  //public float minEnemySpawnDelay = 3f;
  //public float maxEnemySpawnDelay = 10f;

  // private fields
  private float powerUpDelay;
  private float powerupSpawnTimer;

  private float rightEnemySpawnTimer;
  private float leftEnemySpawnTimer;
  private float topEnemySpawnTimer;
  private float bottomEnemySpawnTimer;

  private float rightSpawnDelay;
  private float leftSpawnDelay;
  private float topSpawnDelay;
  private float bottomSpawnDelay;
  private int currentSpawnTier = -1;
  

  private void Start() {
    powerUpDelay = Random.Range(5f, 10f);
    powerupSpawnTimer = 0;

    float minEnemySpawnDelay;
    float maxEnemySpawnDelay;
    GetSpawnDelayRange(out minEnemySpawnDelay, out maxEnemySpawnDelay);

    rightSpawnDelay = Random.Range(minEnemySpawnDelay, maxEnemySpawnDelay);
    leftSpawnDelay = Random.Range(minEnemySpawnDelay, maxEnemySpawnDelay);
    topSpawnDelay = Random.Range(minEnemySpawnDelay, maxEnemySpawnDelay);
    bottomSpawnDelay = Random.Range(minEnemySpawnDelay, maxEnemySpawnDelay);
  }

  // Randomly spawn enemies in the right spawn point
  private void SpawnEnemy() {
    Vector3 enemySpawnPt = new Vector3(
        Random.Range(spawnRange.bounds.min.x, spawnRange.bounds.max.x),
        Random.Range(spawnRange.bounds.min.y, spawnRange.bounds.max.y),
        0);
    Instantiate(enemyPrefab, enemySpawnPt, Quaternion.identity);
  }

  // Randomly spawn enemies in the left spawn point
  private void SpawnEnemyLeft() {
    Vector3 leftEnemySpawnPt = new Vector3(
        Random.Range(spawnRangeLeft.bounds.min.x, spawnRangeLeft.bounds.max.x),
        Random.Range(spawnRangeLeft.bounds.min.y, spawnRangeLeft.bounds.max.y),
        0);
    Instantiate(enemyPrefab, leftEnemySpawnPt, Quaternion.identity);
  }

  // Randomly spawn enemies in the top spawn point
  private void SpawnEnemyTop() {
    Vector3 topEnemySpawnPt = new Vector3(
        Random.Range(spawnRangeTop.bounds.min.x, spawnRangeTop.bounds.max.x),
        Random.Range(spawnRangeTop.bounds.min.y, spawnRangeTop.bounds.max.y),
        0);
    Instantiate(enemyPrefab, topEnemySpawnPt, Quaternion.identity);
  }

  // Randomly spawn enemies in the bottom spawn point
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

  // Method to ramp up the difficulty as the death timer increases
  private void GetSpawnDelayRange(out float minEnemySpawnDelay, out float maxEnemySpawnDelay) {
    minEnemySpawnDelay = 4f;
    maxEnemySpawnDelay = 6f;
    
    if (DeathTimer.Instance == null) {
      if (currentSpawnTier != 0) {
        currentSpawnTier = 0;
        Debug.Log("Spawn Tier 0: DeathTimer not found. Using default delays: " + minEnemySpawnDelay + " to " + maxEnemySpawnDelay);
      }
      return;
    }

    float time = DeathTimer.Instance.currentTime;

    if (time >= 75f ) {
      minEnemySpawnDelay = 1f;
      maxEnemySpawnDelay = 3f;
      if (currentSpawnTier != 3) {
        currentSpawnTier = 3;
        Debug.Log("Spawn Tier 3 activated at time = " + time.ToString("F1") + ". New delay range: " + minEnemySpawnDelay + " to " + maxEnemySpawnDelay);
      }
      
    }
    else if (time >= 60f) {
      minEnemySpawnDelay = 2f;
      maxEnemySpawnDelay = 4f;
      if (currentSpawnTier != 2) {
        currentSpawnTier = 2;
        Debug.Log("Spawn Tier 2 activated at time = " + time.ToString("F1") + ". New delay range: " + minEnemySpawnDelay + " to " + maxEnemySpawnDelay);
      }
    }
    else if (time >= 45f) {
      minEnemySpawnDelay = 3f;
      maxEnemySpawnDelay = 5f;
      if (currentSpawnTier != 1) {
        currentSpawnTier = 1;
        Debug.Log("Spawn Tier 1 activated at time = " + time.ToString("F1") + ". New delay range: " + minEnemySpawnDelay + " to " + maxEnemySpawnDelay);
      }
    }
    else {
      if (currentSpawnTier != 0) {
        currentSpawnTier = 0;
        Debug.Log("Spawn Tier 0: DeathTimer not found. Using default delays: " + minEnemySpawnDelay + " to " + maxEnemySpawnDelay);
      }
    }
  }
  void Update() {
    if (!ui.IsReady) {
      return;
    }

    float minEnemySpawnDelay;
    float maxEnemySpawnDelay;
    GetSpawnDelayRange(out minEnemySpawnDelay, out maxEnemySpawnDelay);
    
    // Right spawn timer
    rightEnemySpawnTimer += Time.deltaTime;
    if (rightEnemySpawnTimer >= rightSpawnDelay) {
      SpawnEnemy();
      rightEnemySpawnTimer = 0.0f;
      rightSpawnDelay = Random.Range(minEnemySpawnDelay, maxEnemySpawnDelay);
    }

    // Left spawn timer
    leftEnemySpawnTimer += Time.deltaTime;
    if (leftEnemySpawnTimer >= leftSpawnDelay) {
      SpawnEnemyLeft();
      leftEnemySpawnTimer = 0.0f;
      leftSpawnDelay = Random.Range(minEnemySpawnDelay, maxEnemySpawnDelay);
    }

    // Top spawn timer
    topEnemySpawnTimer += Time.deltaTime;
    if (topEnemySpawnTimer >= topSpawnDelay) {
      SpawnEnemyTop();
      topEnemySpawnTimer = 0.0f;
      topSpawnDelay = Random.Range(minEnemySpawnDelay, maxEnemySpawnDelay);
    }

    // Bottom spawn timer
    bottomEnemySpawnTimer += Time.deltaTime;
    if (bottomEnemySpawnTimer >= bottomSpawnDelay) {
      SpawnEnemyBottom();
      bottomEnemySpawnTimer = 0.0f;
      bottomSpawnDelay = Random.Range(minEnemySpawnDelay, maxEnemySpawnDelay);
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
