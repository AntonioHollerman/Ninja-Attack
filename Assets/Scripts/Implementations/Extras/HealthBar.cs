using System;
using System.Collections;
using BaseClasses;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Implementations.Extras
{
    public class HealthBar : MonoBehaviour
    {
        public Transform target; // The 2D object to follow
        public float yDisplacement;
        public Image slider;
        public TextMeshProUGUI lvl;

        private void Update()
        {
            transform.position = target.position + Vector3.up * yDisplacement;
        }

        public void UpdateSlider(float fill)
        {
            slider.fillAmount = fill;
        }

        private IEnumerator WaitForTarget()
        {
            yield return new WaitUntil(() => target != null);
            CharacterSheet cs = target.GetChild(0).GetComponent<CharacterSheet>();
            lvl.text = cs.level + "";
        }

        private void Awake()
        {
            StartCoroutine(WaitForTarget());
        }
    }
}