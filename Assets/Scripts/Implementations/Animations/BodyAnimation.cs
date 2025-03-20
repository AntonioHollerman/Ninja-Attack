using System;
using UnityEngine;

namespace Implementations.Animations
{
    public class BodyAnimation : MonoBehaviour
    {
        public GameObject parent;
        void LateUpdate()
        {
            transform.rotation = Quaternion.identity; // Resets rotation to world space
        }
    }
}