using System;
using System.Collections.Generic;
using UnityEngine;

namespace BaseClasses
{
    public abstract class TrackingBehavior : CharacterSheet
    {
        public List<GameObject> targets;
        protected GameObject Target { get; private set;}
        protected float TargetDistance { get; private set;}
        public float detectRange;

        protected override void UpdateWrapper()
        {
            base.UpdateWrapper();
            SetClosestTarget();
            LookAtTarget();
        }

        public void AddTarget(GameObject t)
        {
            targets.Add(t);
        }

        public void RemoveTarget(GameObject t)
        {
            targets.Remove(t);
        }

        private void SetClosestTarget()
        {
            GameObject newTarget = targets[0];
            float smallestDistance = detectRange;
            foreach (var t in targets)
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
            Vector3 targetVec = Target.transform.position;
            Vector3 curVec = transform.position;
            

            // Calculate the direction vector
            Vector3 direction = targetVec - curVec;

            // Ensure the direction vector is not zero
            if (direction != Vector3.zero)
            {
                // Calculate the rotation to face the target
                Quaternion targetRotation = Quaternion.LookRotation(direction);

                // Apply the rotation globally
                transform.rotation = targetRotation;
            }
        }
        private void Start()
        {
            StartWrapper();
        }

        private void Update()
        {
            UpdateWrapper();
        }
    }
}