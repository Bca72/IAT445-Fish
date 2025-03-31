using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDrain : MonoBehaviour
{
    public float drainSpeed = 0.1f; // Units per second
    public float endY = 0f;         // Target Y position to stop at

    private void Update()
    {
        if (transform.position.y > endY)
        {
            transform.position -= new Vector3(0f, drainSpeed * Time.deltaTime, 0f);
        }
    }
}

