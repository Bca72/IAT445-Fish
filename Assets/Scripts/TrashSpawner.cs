using System.Collections;
using System.Collections.Generic;
using DistantLands;
using UnityEngine;

public class TrashSpawner : MonoBehaviour
{
    public GameObject[] trashPrefabs; // Assign different trash objects in the Inspector
    public float spawnHeight = 10f; // How high above the player trash spawns
    public float spawnDistance = 5f; // How far ahead of the player trash starts
    public float spawnXRange = 3f; // Random range for X offset when spawning
    public float dropSpeed = 3f; // How fast trash moves downward
    public float dropDistance = 5f; // How far the trash drops before floating
    public float initialSpawnInterval = 5f; // Starting time between spawns
    public float minSpawnInterval = 1f; // Minimum time between spawns
    public float spawnAcceleration = 0.98f; // Controls how fast spawn interval decreases
    public int maxTrash = 20; // Maximum number of trash objects at once
    public float trashLifetime = 30f; // How long trash stays before disappearing

    private List<GameObject> spawnedTrash = new List<GameObject>();
    private Transform player;
    private float currentSpawnInterval;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentSpawnInterval = initialSpawnInterval;
        StartCoroutine(SpawnTrashRoutine());
    }

    IEnumerator SpawnTrashRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(currentSpawnInterval);
            SpawnTrash();
            currentSpawnInterval = Mathf.Max(minSpawnInterval, currentSpawnInterval * spawnAcceleration);
        }
    }

    void SpawnTrash()
    {
        if (spawnedTrash.Count >= maxTrash)
        {
            DestroyOldestTrash();
        }

        if (trashPrefabs.Length == 0 || player == null) return;

        Vector3 spawnPosition = player.position + (player.forward * spawnDistance);
        spawnPosition += player.right * Random.Range(-spawnXRange, spawnXRange); // Adjust X left/right
        spawnPosition.y += spawnHeight;

        // Only rotate around Y-axis to keep trash upright
        Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);

        GameObject randomTrash = trashPrefabs[Random.Range(0, trashPrefabs.Length)];
        GameObject spawned = Instantiate(randomTrash, spawnPosition, randomRotation);
        spawnedTrash.Add(spawned);

        // Disable floating behavior until drop is complete
        if (spawned.GetComponent<FloatingBottle>() != null)
            spawned.GetComponent<FloatingBottle>().enabled = false;

        if (spawned.GetComponent<FloatingTrash>() != null)
            spawned.GetComponent<FloatingTrash>().enabled = false;

        StartCoroutine(SmoothDrop(spawned, spawnPosition.y - dropDistance));
        Destroy(spawned, trashLifetime);
    }

    IEnumerator SmoothDrop(GameObject trash, float targetY)
    {
        float elapsedTime = 0f;
        float duration = dropDistance / dropSpeed; // Time taken to drop smoothly
        Vector3 startPos = trash.transform.position;
        Vector3 targetPos = new Vector3(startPos.x, targetY, startPos.z);

        while (elapsedTime < duration && trash != null)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            trash.transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        if (trash != null)
        {
            trash.transform.position = targetPos;
            ActivateFloatingBehavior(trash, targetPos);
        }
    }

    void ActivateFloatingBehavior(GameObject trash, Vector3 finalPosition)
    {
        if (trash == null) return;

        // Try to add FloatingBottle or FloatingTrash scripts dynamically if none exist
        if (trash.GetComponent<FloatingBottle>() == null && trash.GetComponent<FloatingTrash>() == null)
        {
            if (Random.value > 0.5f)
            {
                FloatingBottle bottle = trash.AddComponent<FloatingBottle>();
                bottle.SetInitialPosition(finalPosition);
                bottle.enabled = true; // Enable floating after setting position
            }
            else
            {
                FloatingTrash floatingTrash = trash.AddComponent<FloatingTrash>();
                floatingTrash.SetInitialPosition(finalPosition);
                floatingTrash.enabled = true; // Enable floating after setting position
            }
        }
        else
        {
            // Enable existing floating script and set initial position
            if (trash.GetComponent<FloatingBottle>() != null)
            {
                FloatingBottle bottle = trash.GetComponent<FloatingBottle>();
                bottle.SetInitialPosition(finalPosition);
                bottle.enabled = true;
            }

            if (trash.GetComponent<FloatingTrash>() != null)
            {
                FloatingTrash floatingTrash = trash.GetComponent<FloatingTrash>();
                floatingTrash.SetInitialPosition(finalPosition);
                floatingTrash.enabled = true;
            }
        }
    }

    void DestroyOldestTrash()
    {
        if (spawnedTrash.Count > 0)
        {
            GameObject oldestTrash = spawnedTrash[0];
            spawnedTrash.RemoveAt(0);
            Destroy(oldestTrash);
        }
    }
}
