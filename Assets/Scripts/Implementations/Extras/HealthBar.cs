using System;
using UnityEngine;
using UnityEngine.UI;

namespace Implementations.Extras
{
    public class HealthBar : MonoBehaviour
    {
        public Transform target; // The 2D object to follow
        public float yDisplacement;
        public Image slider;

        private void Update()
        {
            transform.position = target.position + Vector3.up * yDisplacement;
        }

        public void UpdateSlider(float fill)
        {
            slider.fillAmount = fill;
        }
    }
}