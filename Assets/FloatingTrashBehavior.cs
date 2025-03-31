using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FloatingTrashBehavior : MonoBehaviour
{
    public float dropSpeed = 1f;
    public float bobHeight = 0.3f;
    public float bobSpeed = 1f;
    public float dropOffsetRange = 0.5f;
    public LayerMask groundMask;            // Assign this to your "Boundary" or "Rock" layer

    private Vector3 initialPos;
    private Vector3 targetDropPos;
    private bool isDropping = true;
    private float dropProgress = 0f;
    private float playerY;

    private float bobStartTime;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            float offset = Random.Range(-dropOffsetRange, dropOffsetRange);
            playerY = playerObj.transform.position.y + offset;
        }
        else
        {
            Debug.LogWarning("Player not found! Tag the player as 'Player'. Using fallback.");
            playerY = transform.position.y - 3f;
        }

        initialPos = transform.position;

        // Do a downward raycast to see if we hit anything below
        Ray ray = new Ray(initialPos, Vector3.down);
        float maxRayDistance = 20f;
        if (Physics.Raycast(ray, out RaycastHit hit, maxRayDistance, groundMask))
        {
            float hoverOffset = 0.3f;
            playerY = Mathf.Max(playerY, hit.point.y + hoverOffset);
        }

        targetDropPos = new Vector3(initialPos.x, playerY, initialPos.z);
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
                initialPos = targetDropPos;
                bobStartTime = Time.time;
            }
        }
        else
        {
            float bobOffset = Mathf.Sin((Time.time - bobStartTime) * bobSpeed) * bobHeight;
            transform.position = initialPos + new Vector3(0, bobOffset, 0);
        }
    }
}




