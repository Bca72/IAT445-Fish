using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleLocomotion : MonoBehaviour
{
    public float moveSpeed = 1.0f;  // Editable speed variable in the Inspector
    public Transform cameraTransform;  // Reference to the VR headset (camera)

    private void Update()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        // Get the forward direction of the camera, ignoring the vertical component
        Vector3 forward = cameraTransform.forward;
        forward.y = 0;  // Keep movement on the horizontal plane
        forward.Normalize();

        // Apply movement
        transform.position += forward * moveSpeed * Time.deltaTime;
    }
}

