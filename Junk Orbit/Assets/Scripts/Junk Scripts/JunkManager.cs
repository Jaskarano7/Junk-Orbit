using System.Collections.Generic;
using UnityEngine;

public class JunkManager : MonoBehaviour
{
    [Header("Script Reference")]
    [SerializeField] private BoundaryScript boundaryScript;

    [Header("Junk Settings")]
    [SerializeField] private List<GameObject> junkPrefabs;
    [SerializeField] private int minJunk = 1;
    [SerializeField] private int maxJunk = 5;
    [SerializeField] private float minJunkDistance = 5f;

    private float spawnRadius;

    private void Start()
    {
        spawnRadius = boundaryScript.BoundaryDimension - 1f;
        SpawnJunk();
    }

    private void SpawnJunk()
    {
        if (junkPrefabs == null || junkPrefabs.Count == 0)
        {
            Debug.LogWarning("JunkManager: No junk prefabs assigned!");
            return;
        }

        int totalJunk = Random.Range(minJunk, maxJunk + 1);
        Debug.Log(totalJunk);
        List<Vector3> placedPositions = new List<Vector3>();

        int maxAttempts = 100;

        for (int i = 0; i < totalJunk; i++)
        {
            bool positionFound = false;

            for (int attempt = 0; attempt < maxAttempts; attempt++)
            {
                // Pick a random angle and random radius
                float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
                float radius = Random.Range(0f, spawnRadius);

                Vector3 spawnPos = transform.position + new Vector3(
                    Mathf.Cos(angle) * radius,
                    0f,
                    Mathf.Sin(angle) * radius
                );

                // Check against all previously placed junk
                bool tooClose = false;
                foreach (var pos in placedPositions)
                {
                    if (Vector3.Distance(pos, spawnPos) < minJunkDistance)
                    {
                        tooClose = true;
                        break;
                    }
                }

                if (!tooClose)
                {
                    // Valid position found
                    int junkIndex = Random.Range(0, junkPrefabs.Count);
                    GameObject spawnedJunk = Instantiate(junkPrefabs[junkIndex], spawnPos, Quaternion.identity, transform);
                    spawnedJunk.name = $"Junk_{i}";
                    placedPositions.Add(spawnPos);
                    positionFound = true;
                    break;
                }
            }

            if (!positionFound)
            {
                Debug.LogWarning($"Could not find valid position for junk {i}");
            }
        }
    }
}
