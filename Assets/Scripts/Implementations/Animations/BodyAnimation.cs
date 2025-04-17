using System;
using UnityEngine;

namespace Implementations.Animations
{
    public class BodyAnimation : MonoBehaviour
    {
        void LateUpdate()
        {
            transform.rotation = Quaternion.identity; // Resets rotation to world space
        }
    }
}