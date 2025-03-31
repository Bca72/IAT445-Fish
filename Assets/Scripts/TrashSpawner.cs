using System.Collections;
using System.Collections.Generic;
using DistantLands;
using UnityEngine;

public class TrashSpawner : MonoBehaviour
{
    public GameObject[] trashPrefabs;
    public Transform player;
    public Transform ceiling;              // Assign your ceiling object here
    public float spawnRadius = 10f;
    public int maxTrashCount = 20;
    public float spawnInterval = 2f;

    private List<GameObject> spawnedTrash = new List<GameObject>();
    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval && spawnedTrash.Count < maxTrashCount)
        {
            SpawnTrash();
            timer = 0f;
        }
    }

    void SpawnTrash()
    {
        if (trashPrefabs.Length == 0 || player == null || ceiling == null) return;

        // Generate forward-biased position within a semi-circle
        Vector2 randomCircle = Random.insideUnitCircle.normalized * Random.Range(0, spawnRadius);
        Vector3 localOffset = new Vector3(randomCircle.x, 0, Mathf.Abs(randomCircle.y));
        Vector3 worldOffset = player.TransformDirection(localOffset);

        Vector3 spawnPosition = player.position + worldOffset;
        spawnPosition.y = ceiling.position.y; // Set Y to ceiling height

        Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
        GameObject prefab = trashPrefabs[Random.Range(0, trashPrefabs.Length)];
        GameObject spawned = Instantiate(prefab, spawnPosition, randomRotation);

        spawnedTrash.Add(spawned);
    }
}

