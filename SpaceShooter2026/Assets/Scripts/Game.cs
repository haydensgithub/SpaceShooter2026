using System;
using System.Diagnostics;
using UnityEngine;

public class Game : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject powerupPrefab;
    public BoxCollider2D spawnRange;
    public BoxCollider2D spawnRangeLeft;
    public BoxCollider2D spawnRangeTop;
    public BoxCollider2D spawnRangeBottom;
    public UI ui;

    private float powerUpDelay;
    private float powerupSpawnTimer;

    private float enemySpawnTimer;
    private float enemySpawnDelay;

    private float currentLoggedNormalizedTime = -1f;

    private void Start()
    {
        powerUpDelay = UnityEngine.Random.Range(5f, 10f);
        powerupSpawnTimer = 0f;

        enemySpawnDelay = GetRandomEnemySpawnDelay();
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