using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishRadialBob : MonoBehaviour
{
    public float bobAmplitude = 0.2f;
    public float bobSpeed = 1.5f;

    private Vector3 baseLocalPos;

    void Start()
    {
        baseLocalPos = transform.localPosition.normalized * transform.localPosition.magnitude;
    }

    void Update()
    {
        float bobOffset = Mathf.Sin(Time.time * bobSpeed) * bobAmplitude;
        Vector3 direction = baseLocalPos.normalized;
        transform.localPosition = baseLocalPos + direction * bobOffset;
    }
}

