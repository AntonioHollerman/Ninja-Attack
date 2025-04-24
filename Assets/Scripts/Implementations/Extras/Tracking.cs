using System;
using System.Collections;
using UnityEngine;

namespace Implementations.Extras
{
    public class Tracking : MonoBehaviour
    {
        public GameObject target;

        private void LateUpdate()
        {
            if (target != null)
            {
                transform.position = new Vector3(target.transform.position.x, target.transform.position.y, -10);
            }
        }
    }
}