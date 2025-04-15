using System;
using UnityEngine;

namespace Implementations.Extras
{
    public class TrackingScript : MonoBehaviour
    {
        public GameObject parent;
        public Vector3 offset;

        private void LateUpdate()
        {
            Vector3 parentForward = parent.transform.forward;
            Vector2 forward2d = new Vector2(parentForward.x, parentForward.z);
            float angle = Mathf.Atan2(forward2d.y, forward2d.x) * Mathf.Rad2Deg;
            
            transform.rotation = Quaternion.Euler(0, angle, 0);
            transform.position = parent.transform.position + offset;
        }
    }
}