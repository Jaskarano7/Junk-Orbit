using System.Collections.Generic;
using UnityEngine;

public class JunkManager : MonoBehaviour
{
    [Header("Script Reference")]
    [SerializeField] private BoundaryScript boundaryScript;

    [Header("Junk Settings")]
    [SerializeField] private List<GameObject> junkPrefabs;
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private int minJunk = 1;
    [SerializeField] private int maxJunk = 5;
    [SerializeField] private float junkMinDistance = 5f;
    [SerializeField] private float junkSpawnRadiusMultiplier = 1f;

    [Header("Obstacle Settings")]
    [SerializeField] private List<GameObject> obstaclePrefabs;
    [SerializeField] private int minObstacles = 1;
    [SerializeField] private int maxObstacles = 3;
    [SerializeField] private float obstacleMinDistance = 8f;
    [SerializeField] private float obstacleSpawnRadiusMultiplier = 0.7f;

    [Header("Spawn Restrictions")]
    [SerializeField] private float innerRadius = 3f;

    [Header("Debug Counters")]
    public int commonCount;
    public int rareCount;
    public int ultraCount;
    public int legendaryCount;

    private float baseRadius;

    private void Start()
    {
        currentLevel = GameSaver.instance.playerData.DifficultyLevel;
        baseRadius = boundaryScript.BoundaryDimension - 2;
        List<Vector3> placedPositions = new List<Vector3>();

        SpawnJunk(placedPositions);
        SpawnObstacles(placedPositions);
    }

    private void SpawnJunk(List<Vector3> placedPositions)
    {
        if (junkPrefabs == null || junkPrefabs.Count == 0) return;

        int totalJunk = Random.Range(minJunk, maxJunk + 1);
        float sectorSize = 360f / totalJunk;
        List<GameObject> allowedPrefabs = GetPrefabsForLevel(junkPrefabs, currentLevel);
        Debug.Log("Current list length: "+allowedPrefabs.Count);

        SpawnObjects(allowedPrefabs, totalJunk, sectorSize, placedPositions, junkMinDistance, baseRadius * junkSpawnRadiusMultiplier);
    }

    private void SpawnObstacles(List<Vector3> placedPositions)
    {
        if (obstaclePrefabs == null || obstaclePrefabs.Count == 0) return;

        int totalObstacles = Random.Range(minObstacles, maxObstacles + 1);
        float sectorSize = 360f / totalObstacles;

        SpawnObjects(obstaclePrefabs, totalObstacles, sectorSize, placedPositions, obstacleMinDistance, baseRadius * obstacleSpawnRadiusMultiplier);
    }

    private void SpawnObjects(List<GameObject> prefabs, int totalCount, float sectorSize, List<Vector3> placedPositions, float minDistance, float spawnRadius)
    {
        int maxAttempts = 20;
        for (int i = 0; i < totalCount; i++)
        {
            bool positionFound = false;

            for (int attempt = 0; attempt < maxAttempts; attempt++)
            {
                float angle = (sectorSize * i + Random.Range(0f, sectorSize)) * Mathf.Deg2Rad;
                float radius = Mathf.Sqrt(Random.Range(0f, 1f)) * spawnRadius;

                if (radius < innerRadius) continue;

                Vector3 spawnPos = transform.position + new Vector3(
                    Mathf.Cos(angle) * radius,
                    0f,
                    Mathf.Sin(angle) * radius
                );

                bool tooClose = false;
                foreach (var pos in placedPositions)
                {
                    if (Vector3.Distance(pos, spawnPos) < minDistance)
                    {
                        tooClose = true;
                        break;
                    }
                }

                if (!tooClose)
                {
                    int prefabIndex = Random.Range(0, prefabs.Count);
                    Instantiate(prefabs[prefabIndex], spawnPos, Quaternion.identity, transform).name = $"Junk_{i}";
                    placedPositions.Add(spawnPos);
                    positionFound = true;
                    break;
                }
            }

            if (!positionFound)
                Debug.LogWarning($"Could not place object {i} without overlap");
        }
    }

    private List<GameObject> GetPrefabsForLevel(List<GameObject> prefabs, int level)
    {
        List<GameObject> allowed = new List<GameObject>();
        if (prefabs == null || prefabs.Count == 0) return allowed;

        int total = prefabs.Count;

        switch (level)
        {
            case 1: // 0-1
                allowed.AddRange(prefabs.GetRange(0, Mathf.Min(2, total)));
                break;
            case 2: // 0-3
                allowed.AddRange(prefabs.GetRange(0, Mathf.Min(4, total)));
                break;
            case 3: // 0-5
                allowed.AddRange(prefabs.GetRange(0, Mathf.Min(6, total)));
                break;
            case 4: // 0-6
            default:
                allowed.AddRange(prefabs.GetRange(0, Mathf.Min(7, total)));
                break;
        }

        return allowed;
    }
}
