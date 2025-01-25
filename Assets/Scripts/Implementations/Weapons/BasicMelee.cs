using System.Collections;
using System.Collections.Generic;
using BaseClasses;
using UnityEngine;

namespace Implementations.Weapons
{
    public class BasicMelee : Weapon
    {
        public int damage; // Damage dealt by the melee attack
        

        protected override void AwakeWrapper()
        {
            base.AwakeWrapper();
            AnimationDuration = frames.Count * secondsBetweenFrame; // Set animation duration to the attack delay
            Attack();
        }

        protected override void Effect(CharacterSheet cs)
        {
            cs.DealDamage(damage); // Apply damage to the target
        }

        protected override IEnumerator Execute()
        {
            yield return null;
        }

        protected override void TriggerEnterWrapper(Collider other)
        {
            base.TriggerEnterWrapper(other);
            BasicArrow arr = other.gameObject.GetComponent<BasicArrow>();
            if (arr != null)
            {
                Destroy(other.gameObject);
            }
        }
    }
}