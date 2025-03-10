using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleLocomotion : MonoBehaviour
{
    public float moveSpeed = 2.0f;              // Normal movement speed
    public float minSpeed = 0.1f;               // Minimum speed when fully inside the boundary
    public float slowingDistance = 2.0f;        // Distance from boundary where slowing starts
    public Transform cameraTransform;           // Reference to the VR headset (camera)

    private float currentSpeed;
    private bool isSlowing = false;
    private float distanceToBoundary = Mathf.Infinity;

    private void Start()
    {
        currentSpeed = moveSpeed;
    }

    private void Update()
    {
        if (isSlowing)
        {
            // Gradually reduce speed based on proximity to boundary
            float slowFactor = Mathf.InverseLerp(slowingDistance, 0, distanceToBoundary);
            currentSpeed = Mathf.Lerp(moveSpeed, minSpeed, slowFactor);
        }
        else
        {
            currentSpeed = moveSpeed;
        }

        MovePlayer();
    }

    private void MovePlayer()
    {
        Vector3 forward = cameraTransform.forward;
        forward.y = 0;
        forward.Normalize();

        transform.position += forward * currentSpeed * Time.deltaTime;
    }

    // Trigger detection for slowing near boundaries
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boundary"))
        {
            isSlowing = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Boundary"))
        {
            // Measure the distance to the nearest boundary point
            distanceToBoundary = Vector3.Distance(transform.position, other.ClosestPoint(transform.position));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Boundary"))
        {
            isSlowing = false;
            distanceToBoundary = Mathf.Infinity;
        }
    }
}

