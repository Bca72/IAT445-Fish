using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WispSpawner : MonoBehaviour
{
    public GameObject wispPrefab;
    public Transform player;
    public Transform checkpoint;
    public float spawnInterval = 3f;

    private float timer;

    void Update()
    {
        if (player == null || checkpoint == null || wispPrefab == null)
            return;

        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnWisp();
            timer = 0f;
        }
    }

    void SpawnWisp()
    {
        GameObject wisp = Instantiate(wispPrefab, player.position, Quaternion.identity);
        GuidingWisp guidingScript = wisp.GetComponent<GuidingWisp>();

        if (guidingScript != null)
        {
            guidingScript.player = player;
            guidingScript.checkpoint = checkpoint;
        }
    }
}
