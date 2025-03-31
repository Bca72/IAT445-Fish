using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTrashBehavior : MonoBehaviour
{
    public float dropSpeed = 1f;
    public float bobHeight = 0.3f;
    public float bobSpeed = 1f;
    public float swayDistance = 0.2f;
    public float swaySpeed = 0.5f;
    public float dropOffsetRange = 0.5f;
    public LayerMask groundMask;

    private Vector3 initialPos;
    private Vector3 targetDropPos;
    private bool isDropping = true;
    private float dropProgress = 0f;

    private float floatTime = 0f;
    private float minBobY = float.NegativeInfinity;

    private Vector3 swayDirection;
    private float swaySeed;
    private Vector3 finalBasePos;
    private Vector3 rotationAxis;
public float rotationSpeed = 10f; // degrees per second


    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        float playerY = playerObj ? playerObj.transform.position.y + Random.Range(-dropOffsetRange, dropOffsetRange)
                                  : transform.position.y - 3f;

        initialPos = transform.position;

        Ray ray = new Ray(initialPos, Vector3.down);
        float maxRayDistance = 20f;
        if (Physics.Raycast(ray, out RaycastHit hit, maxRayDistance, groundMask))
        {
            float hoverOffset = 0.3f;
            playerY = Mathf.Max(playerY, hit.point.y + hoverOffset);
            minBobY = hit.point.y + hoverOffset;
        }

        targetDropPos = new Vector3(initialPos.x, playerY, initialPos.z);

        swayDirection = Random.insideUnitSphere;
        swayDirection.y = 0;
        swayDirection.Normalize();

        swaySeed = Random.Range(0f, 2f * Mathf.PI);
        rotationAxis = new Vector3(
    Random.Range(-1f, 1f),
    Random.Range(-1f, 1f),
    Random.Range(-1f, 1f)
).normalized;

    }

    void Update()
    {
        if (isDropping)
        {
            dropProgress += Time.deltaTime * dropSpeed;
            float t = Mathf.Clamp01(dropProgress);
            float easedT = t * t * (3f - 2f * t);

            transform.position = Vector3.Lerp(initialPos, targetDropPos, easedT);

            if (t >= 1f)
            {
                isDropping = false;
                floatTime = 0f;

                // Offset base position to compensate for sway at t=0
                float initialSwayOffset = Mathf.Sin(swaySeed) * swayDistance;
                Vector3 sway = swayDirection * initialSwayOffset;
                finalBasePos = targetDropPos - new Vector3(sway.x, 0, sway.z);
            }
        }
        else
        {
            floatTime += Time.deltaTime;

            float bobOffset = Mathf.Sin(floatTime * bobSpeed) * bobHeight;
            float nextY = finalBasePos.y + bobOffset;
            if (!float.IsNegativeInfinity(minBobY))
                nextY = Mathf.Max(nextY, minBobY);

            float swayOffset = Mathf.Sin(floatTime * swaySpeed + swaySeed) * swayDistance;
            Vector3 sway = swayDirection * swayOffset;

            transform.position = new Vector3(finalBasePos.x + sway.x, nextY, finalBasePos.z + sway.z);
            transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime, Space.World);

        }
    }
}









