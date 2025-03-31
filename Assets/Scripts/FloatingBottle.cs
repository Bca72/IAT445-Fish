using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingBottle : MonoBehaviour
{
    public float driftRange = 1f;
    public float driftSpeed = 0.2f;
    public Vector2 driftTimeRange = new Vector2(3f, 6f);

    public float bobHeight = 0.3f;
    public float bobSpeed = 1f;

    public float rotationSpeed = 2f;
    public Vector3 rotationAxisMultiplier = new Vector3(1, 0.5f, 1);

    public float transitionSpeed = 1.5f;

    private float _time;
    private Vector3 _originalPos;
    private Vector3 _currentTargetPos;
    private Vector3 _nextTargetPos;
    private Vector3 _rotationChange;
    private Quaternion _currentRotation;
    private Quaternion _nextRotation;

    private bool initialized = false;
    void Start()
{
    if (!initialized)
    {
        _originalPos = transform.position;
        _currentTargetPos = transform.position;
        _currentRotation = transform.rotation;
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

        transform.position = Vector3.Lerp(transform.position, _currentTargetPos, Time.deltaTime * transitionSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, _currentRotation, Time.deltaTime * transitionSpeed);

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
        _nextTargetPos = _originalPos + new Vector3(
            Random.Range(-driftRange, driftRange),
            0,
            Random.Range(-driftRange, driftRange)
        );

        PickNewRotation();
        StartCoroutine(SmoothTransition());
    }

    void PickNewRotation()
    {
        _nextRotation = Quaternion.Euler(
            Random.Range(-5f, 5f) * rotationAxisMultiplier.x,
            Random.Range(0f, 360f) * rotationAxisMultiplier.y,
            Random.Range(-5f, 5f) * rotationAxisMultiplier.z
        );
    }

    IEnumerator SmoothTransition()
    {
        float elapsedTime = 0f;
        float duration = 1f;

        Vector3 startPos = _currentTargetPos;
        Quaternion startRot = _currentRotation;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            _currentTargetPos = Vector3.Lerp(startPos, _nextTargetPos, t);
            _currentRotation = Quaternion.Slerp(startRot, _nextRotation, t);

            yield return null;
        }

        _currentTargetPos = _nextTargetPos;
        _currentRotation = _nextRotation;
    }

    public void SetInitialPosition(Vector3 position)
    {
        transform.position = position;
        _originalPos = position;
        _currentTargetPos = position;
        _currentRotation = transform.rotation;
        initialized = true;
    }
}

