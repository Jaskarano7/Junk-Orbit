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

    [Header("Rarity Percentages (0 to 1)")]
    [Range(0f, 1f)] public float commonRatio = 0.5f;
    [Range(0f, 1f)] public float rareRatio = 0.3f;
    [Range(0f, 1f)] public float ultraRatio = 0.15f;
    [Range(0f, 1f)] public float legendaryRatio = 0.05f;

    [Header("Rarity Weights")]
    public int commonWeight = 50;
    public int rareWeight = 20;
    public int ultraWeight = 5;
    public int legendaryWeight = 1;

    [Header("Debug Counters")]
    public int commonCount;
    public int rareCount;
    public int ultraCount;
    public int legendaryCount;

    private float baseRadius;

    private void Start()
    {
        baseRadius = boundaryScript.BoundaryDimension - 2;
        List<Vector3> placedPositions = new List<Vector3>();

        SpawnJunk(placedPositions);
        SpawnObstacles(placedPositions);

        Debug.Log($"Spawned Junk -> Common: {commonCount}, Rare: {rareCount}, Ultra: {ultraCount}, Legendary: {legendaryCount}");
    }

    private void SpawnJunk(List<Vector3> placedPositions)
    {
        if (junkPrefabs == null || junkPrefabs.Count == 0) return;

        int totalJunk = Random.Range(minJunk, maxJunk + 1);
        float sectorSize = 360f / totalJunk;
        List<GameObject> allowedPrefabs = GetPrefabsForLevel(junkPrefabs, currentLevel);

        SpawnObjects(allowedPrefabs, totalJunk, sectorSize, placedPositions, junkMinDistance, baseRadius * junkSpawnRadiusMultiplier, true);
    }

    private void SpawnObstacles(List<Vector3> placedPositions)
    {
        if (obstaclePrefabs == null || obstaclePrefabs.Count == 0) return;

        int totalObstacles = Random.Range(minObstacles, maxObstacles + 1);
        float sectorSize = 360f / totalObstacles;

        SpawnObjects(obstaclePrefabs, totalObstacles, sectorSize, placedPositions, obstacleMinDistance, baseRadius * obstacleSpawnRadiusMultiplier, false);
    }

    private void SpawnObjects(List<GameObject> prefabs, int totalCount, float sectorSize, List<Vector3> placedPositions, float minDistance, float spawnRadius, bool useRarity)
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
                    if (Vector3.Distance(pos, spawnPos) < minDistance)
                    {
                        tooClose = true;
                        break;
                    }

                if (!tooClose)
                {
                    int prefabIndex = useRarity ? GetWeightedIndex(prefabs.Count) : Random.Range(0, prefabs.Count);
                    Instantiate(prefabs[prefabIndex], spawnPos, Quaternion.identity, transform).name =
                        useRarity ? $"Junk_{i}" : $"Obstacle_{i}";

                    if (useRarity) CountRarity(prefabIndex, prefabs.Count);

                    placedPositions.Add(spawnPos);
                    positionFound = true;
                    break;
                }
            }

            if (!positionFound)
                Debug.LogWarning($"Could not place {(useRarity ? "junk" : "obstacle")} {i} without overlap");
        }
    }

    private List<GameObject> GetPrefabsForLevel(List<GameObject> prefabs, int level)
    {
        List<GameObject> allowed = new List<GameObject>();

        if (prefabs.Count > 0)
            allowed.AddRange(prefabs.GetRange(0, Mathf.Min(3, prefabs.Count)));

        if (level >= 2 && prefabs.Count > 3)
            allowed.AddRange(prefabs.GetRange(3, Mathf.Min(3, prefabs.Count - 3)));

        if (level >= 3 && prefabs.Count > 6)
            allowed.Add(prefabs[prefabs.Count - 1]);

        return allowed;
    }

    private int GetWeightedIndex(int prefabCount)
    {
        int[] weights = new int[prefabCount];

        int commonLimit = Mathf.RoundToInt(prefabCount * commonRatio);
        int rareLimit = Mathf.RoundToInt(prefabCount * (commonRatio + rareRatio));
        int ultraLimit = Mathf.RoundToInt(prefabCount * (commonRatio + rareRatio + ultraRatio));

        for (int i = 0; i < prefabCount; i++)
        {
            if (i < commonLimit) weights[i] = commonWeight;
            else if (i < rareLimit) weights[i] = rareWeight;
            else if (i < ultraLimit) weights[i] = ultraWeight;
            else weights[i] = legendaryWeight;
        }

        int totalWeight = 0;
        foreach (var w in weights) totalWeight += w;
        if (totalWeight == 0) return 0;

        int roll = Random.Range(0, totalWeight);
        int cumulative = 0;
        for (int i = 0; i < weights.Length; i++)
        {
            cumulative += weights[i];
            if (roll < cumulative) return i;
        }

        return 0;
    }

    private void CountRarity(int index, int prefabCount)
    {
        int commonLimit = Mathf.RoundToInt(prefabCount * commonRatio);
        int rareLimit = Mathf.RoundToInt(prefabCount * (commonRatio + rareRatio));
        int ultraLimit = Mathf.RoundToInt(prefabCount * (commonRatio + rareRatio + ultraRatio));

        if (index < commonLimit) commonCount++;
        else if (index < rareLimit) rareCount++;
        else if (index < ultraLimit) ultraCount++;
        else legendaryCount++;
    }
}
