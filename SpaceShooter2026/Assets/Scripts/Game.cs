using System;
using System.Diagnostics;
using UnityEngine;

public class Game : MonoBehaviour {
  // set in inspector
  public GameObject fastEnemyPrefab;
  public GameObject tankEnemyPrefab;
  public GameObject powerupPrefab;
  public BoxCollider2D spawnRange;
  public BoxCollider2D spawnRangeLeft;
  public BoxCollider2D spawnRangeTop;
  public BoxCollider2D spawnRangeBottom;
  public UI ui;

  // private fields
  private float powerUpDelay;
  private float powerupSpawnTimer;

    private float currentLoggedNormalizedTime = -1f;

    private void Start()
    {
        powerUpDelay = UnityEngine.Random.Range(5f, 10f);
        powerupSpawnTimer = 0f;

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

  // Choose which enemy prefab to spawn
  private GameObject GetRandomEnemyPrefab() {
    float roll = Random.value;

    // 75% chance for fast enemy, 25% chance for tank
    if (roll <= 0.75f) {
      return fastEnemyPrefab;
    }
    else {
      return tankEnemyPrefab;
    }
  }
  // Randomly spawn enemies in the right spawn point
  private void SpawnEnemy() {
    Vector3 enemySpawnPt = new Vector3(
        Random.Range(spawnRange.bounds.min.x, spawnRange.bounds.max.x),
        Random.Range(spawnRange.bounds.min.y, spawnRange.bounds.max.y),
        0);
    Instantiate(GetRandomEnemyPrefab(), enemySpawnPt, Quaternion.identity);
  }

  // Randomly spawn enemies in the left spawn point
  private void SpawnEnemyLeft() {
    Vector3 leftEnemySpawnPt = new Vector3(
        Random.Range(spawnRangeLeft.bounds.min.x, spawnRangeLeft.bounds.max.x),
        Random.Range(spawnRangeLeft.bounds.min.y, spawnRangeLeft.bounds.max.y),
        0);
    Instantiate(GetRandomEnemyPrefab(), leftEnemySpawnPt, Quaternion.identity);
  }

  // Randomly spawn enemies in the top spawn point
  private void SpawnEnemyTop() {
    Vector3 topEnemySpawnPt = new Vector3(
        Random.Range(spawnRangeTop.bounds.min.x, spawnRangeTop.bounds.max.x),
        Random.Range(spawnRangeTop.bounds.min.y, spawnRangeTop.bounds.max.y),
        0);
    Instantiate(GetRandomEnemyPrefab(), topEnemySpawnPt, Quaternion.identity);
  }

  // Randomly spawn enemies in the bottom spawn point
  private void SpawnEnemyBottom() {
    Vector3 bottomEnemySpawnPt = new Vector3(
        Random.Range(spawnRangeBottom.bounds.min.x, spawnRangeBottom.bounds.max.x),
        Random.Range(spawnRangeBottom.bounds.min.y, spawnRangeBottom.bounds.max.y),
        0);
    Instantiate(GetRandomEnemyPrefab(), bottomEnemySpawnPt, Quaternion.identity);
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

    private Vector3 GetRandomPointInBox(BoxCollider2D box)
    {
        return new Vector3(
          UnityEngine.Random.Range(box.bounds.min.x, box.bounds.max.x),
          UnityEngine.Random.Range(box.bounds.min.y, box.bounds.max.y),
          0f
        );
    }

    private void SpawnEnemyAt(BoxCollider2D box)
    {
        Instantiate(enemyPrefab, GetRandomPointInBox(box), Quaternion.identity);
    }

    private void SpawnEnemyRandomSide()
    {
        int spawnIndex = UnityEngine.Random.Range(0, 4);

        switch (spawnIndex)
        {
            case 0:
                SpawnEnemyAt(spawnRange);
                break;
            case 1:
                SpawnEnemyAt(spawnRangeLeft);
                break;
            case 2:
                SpawnEnemyAt(spawnRangeTop);
                break;
            default:
                SpawnEnemyAt(spawnRangeBottom);
                break;
        }
    }

    private void SpawnPowerup()
    {
        Vector3 powerupSpawnPt = new Vector3(
          UnityEngine.Random.Range(spawnRange.bounds.min.x, spawnRange.bounds.max.x),
          UnityEngine.Random.Range(spawnRange.bounds.min.y, spawnRange.bounds.max.y),
          0f
        );
        Instantiate(powerupPrefab, powerupSpawnPt, Quaternion.identity);
    }

    private float GetDifficulty01()
    {
        if (DeathTimer.Instance == null)
        {
            return 0f;
        }

        float time = DeathTimer.Instance.currentTime;
        return Mathf.Clamp01(time / 75f);
    }

    private float GetRandomEnemySpawnDelay()
    {
        float t = GetDifficulty01();

        float minDelay = Mathf.Lerp(.5f, 0.25f, t);
        float maxDelay = Mathf.Lerp(1f, 0.5f, t);

        float roundedT = Mathf.Round(t * 10f) / 10f;
        if (!Mathf.Approximately(roundedT, currentLoggedNormalizedTime))
        {
            currentLoggedNormalizedTime = roundedT;
            //Debug.Log("Spawn difficulty = " + roundedT.ToString("F1") + ", delay range: " + minDelay.ToString("F2") + " to " + maxDelay.ToString("F2"));
        }

        return UnityEngine.Random.Range(minDelay, maxDelay);
    }

    private void Update()
    {
        if (!ui.IsReady)
        {
            return;
        }

        enemySpawnTimer += Time.deltaTime;
        if (enemySpawnTimer >= enemySpawnDelay)
        {
            SpawnEnemyRandomSide();
            enemySpawnTimer = 0f;
            enemySpawnDelay = GetRandomEnemySpawnDelay();
        }

        powerupSpawnTimer += Time.deltaTime;
        if (powerupSpawnTimer >= powerUpDelay)
        {
            SpawnPowerup();
            powerUpDelay = UnityEngine.Random.Range(5f, 10f);
            powerupSpawnTimer = 0f;
        }
    }
}