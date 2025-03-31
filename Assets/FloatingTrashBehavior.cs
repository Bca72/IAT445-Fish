using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTrashBehavior : MonoBehaviour
{
    public float dropSpeed = 1f;
    public float bobHeight = 0.3f;
    public float bobSpeed = 1f;
    public float dropOffsetRange = 0.5f;

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
            Debug.LogWarning("Player not found! Tag the player as 'Player'. Using current Y.");
            playerY = transform.position.y - 3f;
        }

        initialPos = transform.position;
        targetDropPos = new Vector3(initialPos.x, playerY, initialPos.z);
    }

    void Update()
    {
        if (isDropping)
        {
            dropProgress += Time.deltaTime * dropSpeed;

            float t = Mathf.Clamp01(dropProgress);
            float easedT = t * t * (3f - 2f * t); // smoothstep easing

            transform.position = Vector3.Lerp(initialPos, targetDropPos, easedT);

            if (t >= 1f)
            {
                isDropping = false;
                initialPos = targetDropPos;
                bobStartTime = Time.time; // capture time to sync bob phase
            }
        }
        else
        {
            float bobOffset = Mathf.Sin((Time.time - bobStartTime) * bobSpeed) * bobHeight;
            transform.position = initialPos + new Vector3(0, bobOffset, 0);
        }
    }
}



