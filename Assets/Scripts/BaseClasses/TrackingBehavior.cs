using System;
using System.Collections.Generic;
using UnityEngine;

namespace BaseClasses
{
    public abstract class TrackingBehavior : CharacterSheet
    {
        private List<GameObject> _targets;
        protected GameObject Target { get; private set;}
        protected float TargetDistance { get; private set;}
        public float detectRange;

        protected override void UpdateWrapper()
        {
            base.UpdateWrapper();
            
            if (!IsStunned)
            {
                SetClosestTarget();
                LookAtTarget();
            }
        }

        protected override void AwakeWrapper()
        {
            base.AwakeWrapper();
            _targets = new List<GameObject>();
        }

        public void AddTarget(GameObject t)
        {
            if (t == null)
            {
                return;
            }
            _targets.Add(t);
        }

        public void RemoveTarget(GameObject t)
        {
            _targets.Remove(t);
        }

        private void SetClosestTarget()
        {
            if (_targets.Count <= 0)
            {
                return;
            }
            GameObject newTarget = _targets[0];
            float smallestDistance = detectRange;
            foreach (var t in _targets)
            {
                float distance = Mathf.Abs((transform.position - t.transform.position).magnitude);
                if (distance <= smallestDistance)
                {
                    newTarget = t;
                    smallestDistance = distance;
                }
            }

            Target = newTarget;
            TargetDistance = smallestDistance;
        }

        private void LookAtTarget()
        {
            if (Target == null)
            {
                return;
            }
            Vector3 targetVec = Target.transform.position;
            Vector3 curVec = transform.position;
            

            // Calculate the direction vector
            Vector3 direction = targetVec - curVec;

            // Ensure the direction vector is not zero
            if (direction != Vector3.zero)
            {
                // Calculate the rotation to face the target
                Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.forward);

                // Apply the rotation globally
                transform.rotation = targetRotation;
            }
        }
    }
}