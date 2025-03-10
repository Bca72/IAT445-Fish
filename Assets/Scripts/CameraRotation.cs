using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    public float rotationSpeed = 100.0f; // Speed of rotation

    void Update()
    {
        RotateCamera();
    }

    private void RotateCamera()
    {
        float horizontal = Input.GetAxis("Horizontal"); // A/D for left/right rotation
        float vertical = Input.GetAxis("Vertical");     // W/S for up/down rotation

        // Rotate around the Y-axis for horizontal movement (left/right)
        transform.Rotate(Vector3.up, horizontal * rotationSpeed * Time.deltaTime);

        // Rotate around the X-axis for vertical movement (up/down)
        transform.Rotate(Vector3.right, -vertical * rotationSpeed * Time.deltaTime);
    }
}

