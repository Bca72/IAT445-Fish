using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DistantLands
{
    public class FloatingTrash : MonoBehaviour
    {
        public float driftRange = 1f;  // How far the bottle drifts
        public float driftSpeed = 0.2f; // How fast it drifts
        public Vector2 driftTimeRange = new Vector2(3f, 6f); // Time before picking a new position

        public float bobHeight = 0.3f; // How high it bobs up and down
        public float bobSpeed = 1f;    // How fast it bobs

        public float transitionSpeed = 1.5f; // Smooth transition speed

        public Vector3 currentDirection = new Vector3(1, 0, 0); // **Direction of ocean current**
        public float currentStrength = 0.05f; // **Strength of the ocean current**

        private float _time;
        private Vector3 _originalPos;
        private Vector3 _currentTargetPos;
        private Vector3 _nextTargetPos;

        void Awake()
        {
            if (_originalPos == Vector3.zero)
            {
                _originalPos = transform.position; // Set this only if not set by SetInitialPosition
            }

            _time = Random.Range(0, driftTimeRange.y);
            PickNewTarget();
        }

        void Update()
        {
            // Bobbing effect (smooth up and down motion)
            float bobOffset = Mathf.Sin(Time.time * bobSpeed) * bobHeight;
            transform.position = new Vector3(transform.position.x, _originalPos.y + bobOffset, transform.position.z);

            // Apply ocean current movement (slow global push)
            transform.position += currentDirection.normalized * currentStrength * Time.deltaTime;

            // Smoothly move to target position (random drifting)
            transform.position = Vector3.Lerp(transform.position, _currentTargetPos, Time.deltaTime * transitionSpeed);

            // Timer for changing drift direction
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

            // **New target is influenced by the current direction**
            _nextTargetPos = transform.position + (currentDirection.normalized * currentStrength * Random.Range(1f, 5f)) +
                new Vector3(
                    Random.Range(-driftRange, driftRange),
                    0,
                    Random.Range(-driftRange, driftRange)
                );

            // Start smooth transition
            StartCoroutine(SmoothTransition());
        }

        IEnumerator SmoothTransition()
        {
            float elapsedTime = 0f;
            float duration = 2f; // **Slower transitions make it feel more natural**

            Vector3 startPos = _currentTargetPos;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / duration;

                // Smooth transition using Lerp for position
                _currentTargetPos = Vector3.Lerp(startPos, _nextTargetPos, t);

                yield return null;
            }

            _currentTargetPos = _nextTargetPos;
        }

        public void SetInitialPosition(Vector3 position)
        {
            transform.position = position;
            _originalPos = position; // Ensure it starts floating from where it landed
            _currentTargetPos = position; // Fix target position to prevent snapping
        }
    }
}
