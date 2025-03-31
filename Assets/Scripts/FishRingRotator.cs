using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FishRingRotator : MonoBehaviour
{
    public float rotationSpeed = 20f;

    void Update()
    {
        transform.Rotate(Vector3.right, rotationSpeed * Time.deltaTime); // X-axis


    }
}

