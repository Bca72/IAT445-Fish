using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleLocomotion : MonoBehaviour
{
    public float moveSpeed = 2.0f;
    public float minSpeed = 0.1f;
    public float slowingDistance = 2.0f;
    public float stopDistance = 0.05f;
    public Transform cameraTransform;
    public LayerMask boundaryLayer;

    private float currentSpeed;

    private void Update()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        Vector3 forward = cameraTransform.forward;
        forward.Normalize();

        if (IsApproachingBoundary(forward, out float distance))
        {
            if (distance <= stopDistance)
            {
                currentSpeed = 0;
                Debug.Log("Stopped at boundary.");
            }
            else
            {
                float slowFactor = Mathf.Clamp01(distance / slowingDistance);
                currentSpeed = Mathf.Lerp(minSpeed, moveSpeed, slowFactor);
                Debug.Log($"Slowing down. Distance: {distance}, Speed: {currentSpeed}");
            }
        }
        else
        {
            currentSpeed = moveSpeed;
        }

        transform.position += forward * currentSpeed * Time.deltaTime;
    }

    private bool IsApproachingBoundary(Vector3 direction, out float distance)
    {
        RaycastHit hit;

        // Cast a ray to detect boundary in the moving direction
        if (Physics.Raycast(transform.position, direction, out hit, slowingDistance, boundaryLayer))
        {
            distance = hit.distance;
            return true;
        }

        distance = Mathf.Infinity;
        return false;
    }
}
