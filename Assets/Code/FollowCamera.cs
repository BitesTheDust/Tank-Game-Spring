using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TankGame
{
    public class FollowCamera : MonoBehaviour, ICameraFollow
    {
        // Target to follow, set in editor
        [SerializeField] private Transform _target;
        // Distance from target, negative values go underground
        [SerializeField] private float _distance;
        [SerializeField] private float _angle;

        // Sets _angle.
        public void SetAngle(float angle)
        {
            _angle = angle;
        }

        // Sets _distance.
        public void SetDistance(float distance)
        {
            _distance = distance;
        }

        // Sets _target.
        public void SetTarget(Transform targetTransform)
        {
            _target = targetTransform;
        }

        private void LateUpdate()
        {
            // proceed only if we have a target to follow.
            if (_target != null)
            {
                // Calculate unknown angle
                float lastAngle = 90 - _angle;

                // Determine other sides of position triangle.
                float xPos = (_distance * Mathf.Sin(_angle)) / Mathf.Sin(90);
                float yPos = (-_distance * Mathf.Sin(90 - _angle)) / Mathf.Sin(90);

                // Find position based on target position + calculated position.
                Vector3 desiredPosition = _target.position + (_target.forward * xPos) + (Vector3.up * yPos);
                transform.position = desiredPosition;

                // Turn to face target.
                transform.rotation = Quaternion.LookRotation(Vector3.Normalize(_target.position - transform.position), Vector3.up);
            }
        }
    }

}
