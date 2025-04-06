using System;
using UnityEngine;

namespace Scenes.Testing
{
    public class Position : MonoBehaviour
    {
        public Vector3 pos;
        public GameObject toCompare;

        private void Update()
        {
            pos = transform.position;
        }

        public void Displacement()
        {
            Debug.Log($"Position: {transform.position}");
            Debug.Log($"X Displacement: {Math.Abs(toCompare.transform.position.x - transform.position.x)}");
        }
    }
}