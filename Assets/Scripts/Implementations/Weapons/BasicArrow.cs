using System.Collections;
using UnityEngine;

namespace Implementations.Weapons
{
    public class BasicArrow : Weapon
    {
        public int damage;
        public float speed = 10f; // Speed of the arrow
        public float maxTravelDistance = 20f; // Maximum distance the arrow can travel before deactivating

        private Vector2 startPosition;

        protected override void StartWrapper()
        {
            base.StartWrapper();
            AnimationDuration = maxTravelDistance / speed; // Calculate duration based on speed and distance
            Ignore = new List<CharacterSheet> { ParentCs }; // Exclude the parent character
        }

        protected override void Effect(CharacterSheet cs)
        {
            cs.DealDamage(damage); // Apply damage to the target
            gameObject.SetActive(false); // Deactivate the arrow after hitting a target
        }

        protected override IEnumerator PlayAnimation()
        {
            startPosition = transform.position; // Record the starting position of the arrow

            // Move the arrow downward in a straight line
            while (Vector2.Distance(startPosition, transform.position) < maxTravelDistance)
            {
                // Move the arrow downward based on speed
                transform.Translate(Vector2.down * speed * Time.deltaTime);

                yield return new WaitForSeconds(0.2f); // Wait for the next frame
            }

            // Deactivate the arrow after it has traveled the maximum distance
            gameObject.SetActive(false);
        }
    }
}