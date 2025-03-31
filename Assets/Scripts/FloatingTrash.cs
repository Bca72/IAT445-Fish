using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTrash : MonoBehaviour
{
    public float driftRange = 1f;
    public float driftSpeed = 0.2f;
    public Vector2 driftTimeRange = new Vector2(3f, 6f);

    public float bobHeight = 0.3f;
    public float bobSpeed = 1f;

    public float transitionSpeed = 1.5f;

    public Vector3 currentDirection = new Vector3(1, 0, 0);
    public float currentStrength = 0.05f;

    private float _time;
    private Vector3 _originalPos;
    private Vector3 _currentTargetPos;
    private Vector3 _nextTargetPos;

    private bool initialized = false;

    void Start()
{
    if (!initialized)
    {
        _originalPos = transform.position;
        _currentTargetPos = transform.position;
        initialized = true;

        _time = Random.Range(0, driftTimeRange.y);
        PickNewTarget();
    }
}

    void OnEnable()
    {
        if (!initialized) return;

        _time = Random.Range(0, driftTimeRange.y);
        PickNewTarget();
    }

    void Update()
    {
        if (!initialized) return;

        float bobOffset = Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = new Vector3(transform.position.x, _originalPos.y + bobOffset, transform.position.z);

        transform.position += currentDirection.normalized * currentStrength * Time.deltaTime;

        transform.position = Vector3.Lerp(transform.position, _currentTargetPos, Time.deltaTime * transitionSpeed);

        if (_time > 0)
        {
            _time -= Time.deltaTime;
        }
        else
        {
            PickNewTarget();
        }
    }

    void PickNewTarget()
    {
        _time = Random.Range(driftTimeRange.x, driftTimeRange.y);

        _nextTargetPos = transform.position + (currentDirection.normalized * currentStrength * Random.Range(1f, 5f)) +
            new Vector3(
                Random.Range(-driftRange, driftRange),
                0,
                Random.Range(-driftRange, driftRange)
            );

        StartCoroutine(SmoothTransition());
    }

    IEnumerator SmoothTransition()
    {
        float elapsedTime = 0f;
        float duration = 2f;
        Vector3 startPos = _currentTargetPos;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            _currentTargetPos = Vector3.Lerp(startPos, _nextTargetPos, t);
            yield return null;
        }

        _currentTargetPos = _nextTargetPos;
    }

    public void SetInitialPosition(Vector3 position)
    {
        transform.position = position;
        _originalPos = position;
        _currentTargetPos = position;
        initialized = true;
    }
}

