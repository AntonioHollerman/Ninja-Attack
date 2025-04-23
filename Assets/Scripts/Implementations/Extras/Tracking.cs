using System;
using UnityEngine;

namespace Implementations.Extras
{
    public class Tracking : MonoBehaviour
    {
        public GameObject target;

        private void LateUpdate()
        {
            transform.position = new Vector3(target.transform.position.x, target.transform.position.y, -10);
        }
    }
}