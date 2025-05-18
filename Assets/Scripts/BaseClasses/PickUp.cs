using System;
using Implementations.Animations;
using Implementations.Animations.UniqueAnimation;
using UnityEngine;

namespace BaseClasses
{
    public abstract class PickUp : MonoBehaviour
    {
        public abstract void Effect(Player cs);

        private void OnTriggerEnter(Collider other)
        {
            Player cs = other.GetComponent<Player>();
            if (cs != null)
            {
                Effect(cs);
                Destroy(gameObject);
            }
        }
    }
}