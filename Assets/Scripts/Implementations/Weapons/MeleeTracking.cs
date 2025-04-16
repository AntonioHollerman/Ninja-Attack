using System;
using UnityEngine;

namespace Implementations.Weapons
{
    // Parent Roc: (-90, 0, 180)
    // Body Roc: (90, 0, 0)
    public class MeleeTracking : MonoBehaviour
    {
        public GameObject parent;
        public float zOffset;
        
        private void LateUpdate()
        {
            transform.position = parent.transform.position;
            transform.rotation = parent.transform.rotation;
            transform.Translate(Vector3.forward * zOffset);

            transform.rotation = Quaternion.LookRotation(parent.transform.forward, Vector3.forward);
        }
    }
}