using System.Collections.Generic;
using UnityEngine;

public class JunkManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> Junk;
    [SerializeField] private int MinJunk = 1;
    [SerializeField] private int MaxJunk = 5;

    [SerializeField] private Transform junkParent; // optional - where to put spawned junk

    private void Start()
    {
        SpawnJunk();
    }

    private void SpawnJunk()
    {
        Debug.Log("Spawning Junk");

        int totalJunk = Random.Range(MinJunk, MaxJunk + 1); // inclusive max
        for (int i = 0; i < totalJunk; i++)
        {
            int junkIndex = Random.Range(0, Junk.Count);
            Debug.Log($"Spawning Junk {junkIndex}");

            // Optionally randomize position around a center point
            Vector3 spawnPos = transform.position + new Vector3(
                Random.Range(-2f, 2f),
                Random.Range(-2f, 2f),
                0f);

            Instantiate(Junk[junkIndex], spawnPos, Quaternion.identity, junkParent);
        }
    }
}
