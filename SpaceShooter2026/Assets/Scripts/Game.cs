using System;
using System.Diagnostics;
using UnityEngine;

public class Game : MonoBehaviour
{
    public GameObject fastEnemyPrefab;
    public GameObject tankEnemyPrefab;
    public GameObject sharkEnemyPrefab;
    public GameObject powerupPrefab;
    public BoxCollider2D spawnRange;
    public BoxCollider2D spawnRangeLeft;
    public BoxCollider2D spawnRangeTop;
    public BoxCollider2D spawnRangeBottom;
    public float sharkPhaseStartTime = 60f;
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

        enemySpawnTimer = 0f;
        enemySpawnDelay = GetRandomEnemySpawnDelay();
    }

    private GameObject GetRandomEnemyPrefab()
    {
        if (DeathTimer.Instance == null)
        {
            return fastEnemyPrefab;
        }

        float time = DeathTimer.Instance.currentTime;

        if (time >= 75f)
        {
            return sharkEnemyPrefab;
        }
        else if (time >= 50f)
        {
            float roll = UnityEngine.Random.value;
            if (roll <= 0.65f)
                return fastEnemyPrefab;
            else
                return tankEnemyPrefab;
        }
        else
        {
            float roll = UnityEngine.Random.value;
            if (roll <= 0.85f)
                return fastEnemyPrefab;
            else
                return tankEnemyPrefab;
        }
        
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
        Instantiate(GetRandomEnemyPrefab(), GetRandomPointInBox(box), Quaternion.identity);
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

        float minDelay = Mathf.Lerp(1f, 0.20f, t);
        float maxDelay = Mathf.Lerp(1.5f, .25f, t);

        // Slow down spawns in the shark phase to make it more manageable
        if (DeathTimer.Instance != null && DeathTimer.Instance.currentTime >= sharkPhaseStartTime)
        {
            minDelay = .75f;
            maxDelay = 1.5f;
        }

        float roundedT = Mathf.Round(t * 10f) / 10f;
        if (!Mathf.Approximately(roundedT, currentLoggedNormalizedTime))
        {
            currentLoggedNormalizedTime = roundedT;
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