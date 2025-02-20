using System;
using UnityEngine;

namespace BaseClasses
{
    public abstract class PickUp : MonoBehaviour
    {
        public abstract void Effect(CharacterSheet cs);

        private void OnTriggerEnter(Collider other)
        {
            CharacterSheet cs = other.GetComponent<CharacterSheet>();
            if (cs != null)
            {
                Effect(cs);
                Destroy(gameObject);
            }
        }
    }
}