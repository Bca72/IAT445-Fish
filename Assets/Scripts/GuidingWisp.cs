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
    private Transform fishModel;

    private void Start()
    {
        if (player != null)
        {
            transform.position = player.position;
            Invoke(nameof(StartMoving), startDelay);
        }

        // Get the fish child model (assuming it's the first child)
        if (transform.childCount > 0)
            fishModel = transform.GetChild(0);
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

            // 🐟 Rotate the fish child to face movement direction
            if (fishModel != null && direction != Vector3.zero)
            {
                fishModel.rotation = Quaternion.LookRotation(direction, Vector3.up);
            }
        }
        else
        {
            Destroy(gameObject, 2f);
        }
    }
}

