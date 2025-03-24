using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DistantLands
{
    public class FloatingBottle : MonoBehaviour
    {
        public float driftRange = 1f;  // How far the bottle drifts
        public float driftSpeed = 0.2f; // How fast it drifts
        public Vector2 driftTimeRange = new Vector2(3f, 6f); // Time before picking a new position

        public float bobHeight = 0.3f; // How high it bobs up and down
        public float bobSpeed = 1f;    // How fast it bobs

        public float rotationSpeed = 2f; // Rotation speed
        public Vector3 rotationAxisMultiplier = new Vector3(1, 0.5f, 1); // Tilt and rotate in different axes

        public float transitionSpeed = 1.5f; // **Smooth transition speed for movement & rotation**

        private float _time;
        private Vector3 _originalPos;
        private Vector3 _currentTargetPos;
        private Vector3 _nextTargetPos;
        private Vector3 _rotationChange;
        private Quaternion _currentRotation;
        private Quaternion _nextRotation;

        void Awake()
        {
            _originalPos = transform.position;
            PickNewTarget();
        }

        void Update()
        {
            // Bobbing effect (smooth up and down motion)
            float bobOffset = Mathf.Sin(Time.time * bobSpeed) * bobHeight;
            transform.position = new Vector3(transform.position.x, _originalPos.y + bobOffset, transform.position.z);

            // Smoothly move to target position
            transform.position = Vector3.Lerp(transform.position, _currentTargetPos, Time.deltaTime * transitionSpeed);

            // Smoothly rotate to new direction
            transform.rotation = Quaternion.Slerp(transform.rotation, _currentRotation, Time.deltaTime * transitionSpeed);

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
            _nextTargetPos = _originalPos + new Vector3(
                Random.Range(-driftRange, driftRange),
                0, // No vertical movement for drift, handled by bobbing
                Random.Range(-driftRange, driftRange)
            );

            PickNewRotation();

            // Instead of snapping instantly, we smoothly transition over time
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
            float duration = 1f; // Adjust this to control smoothness

            Vector3 startPos = _currentTargetPos;
            Quaternion startRot = _currentRotation;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / duration;

                // Smooth transition using Lerp for position and Slerp for rotation
                _currentTargetPos = Vector3.Lerp(startPos, _nextTargetPos, t);
                _currentRotation = Quaternion.Slerp(startRot, _nextRotation, t);

                yield return null;
            }

            // Ensure final values match exactly
            _currentTargetPos = _nextTargetPos;
            _currentRotation = _nextRotation;
        }
    }
}
