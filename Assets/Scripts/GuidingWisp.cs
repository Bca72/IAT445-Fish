using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidingWisp : MonoBehaviour
{
    public Transform player;
    public Transform checkpoint;
    public float speed = 6f;
    public float startDelay = 1f;

    private bool isMoving = false;

    private void Start()
    {
        if (player != null)
        {
            transform.position = player.position;
            Invoke(nameof(StartMoving), startDelay);
        }
    }

    void StartMoving()
    {
        isMoving = true;
    }

    void Update()
    {
        if (!isMoving || checkpoint == null) return;

        Vector3 direction = (checkpoint.position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, checkpoint.position);

        if (distance > 0.5f)
        {
            transform.position += direction * speed * Time.deltaTime;
        }
        else
        {
            // Optional: destroy or fade out at destination
            Destroy(gameObject, 2f);
        }
    }
}
