using System;
using UnityEngine;

namespace Scenes.Testing
{
    public class PrintStuff : MonoBehaviour
    {
        public GameObject startIcon;
        public GameObject secondIcon;

        private void Awake()
        {
            Debug.Log($"Start Pos: {startIcon.transform.position}");
            Debug.Log($"Displacement: {Math.Abs(startIcon.transform.position.x - secondIcon.transform.position.x)}");
        }
    }
}