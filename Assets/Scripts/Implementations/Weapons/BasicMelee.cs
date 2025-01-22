using System.Collections;
using System.Collections.Generic;
using BaseClasses;
using UnityEngine;

namespace Implementations.Weapons
{
    public class BasicMelee : Weapon
    {
        public int damage; // Damage dealt by the melee attack
        public float attackRange = 1.5f; // Range of the melee attack
        public float attackDelay = 0.5f; // Delay between attacks

        private float _attackTimer; // Timer to manage attack delay

        protected override void StartWrapper()
        {
            base.StartWrapper();
            AnimationDuration = attackDelay; // Set animation duration to the attack delay
        }

        protected override void Effect(CharacterSheet cs)
        {
            cs.DealDamage(damage); // Apply damage to the target
        }

        protected override IEnumerator PlayAnimation()
        {
            // Start the attack animation (you can add your animation logic here)
            // For example, you might want to play an animation using an Animator component

            // Wait for the attack delay before executing the attack
            yield return new WaitForSeconds(attackDelay);

            // Check for targets within range
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, attackRange);
            foreach (var hitCollider in hitColliders)
            {
                CharacterSheet cs = hitCollider.GetComponent<CharacterSheet>();
                if (cs != null && !ignore.Contains(cs))
                {
                    Effect(cs); // Apply the effect (damage) to the character
                }
            }

        }

        private void Update()
        {
            // Decrement the attack timer
            if (_attackTimer > 0)
            {
                _attackTimer -= Time.deltaTime;
            }
        }

        public void ExecuteMeleeAttack()
        {
            if (_attackTimer <= 0)
            {
                _attackTimer = attackDelay; // Reset the attack timer
                Execute(); // Call the Execute method to start the attack
            }
        }
    }
}