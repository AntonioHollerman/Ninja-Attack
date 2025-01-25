using System.Collections;
using System.Collections.Generic;
using BaseClasses;
using UnityEngine;

namespace Implementations.Weapons
{
    public class BasicArrow : Weapon
    {
        public int damage;
        public float speed = 5f; // Speed of the arrow
        public float maxTravelDistance = 15f; // Maximum distance the arrow can travel before deactivating

        protected override void StartWrapper()
        {
            base.StartWrapper();
            
            AnimationDuration = speed != 0 ? maxTravelDistance / speed : 3; // Calculate duration based on speed and distance
            Attack();
        }

        protected override void Effect(CharacterSheet cs)
        {
            cs.DealDamage(damage); // Apply damage to the target
            Deactivate();
        }

        protected override IEnumerator Execute()
        {
            while (true)
            {
                transform.Translate(speed * Time.deltaTime * Vector3.forward);
                yield return null;
            }
        }
    }
}