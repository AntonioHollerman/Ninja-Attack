using System.Collections;
using System.Collections.Generic;
using BaseClasses;
using UnityEngine;

namespace Implementations.Weapons
{
    public class BasicArrow : Weapon
    {
        public int damage;
        public float speed = 10f; // Speed of the arrow
        public float maxTravelDistance = 20f; // Maximum distance the arrow can travel before deactivating
        public List<GameObject> ignore;

        private Vector2 startPosition;

        protected override void StartWrapper()
        {
            base.StartWrapper();
            
            AnimationDuration = maxTravelDistance / speed; // Calculate duration based on speed and distance
            Ignore = new List<CharacterSheet> { ParentCs }; // Exclude the parent character
            foreach (var go in ignore)
            {
                CharacterSheet cs = go.GetComponent<CharacterSheet>();
                if (cs != null)
                {
                    Ignore.Add(cs);
                }
            }
            Execute();
        }

        protected override void UpdateWrapper()
        {
            base.UpdateWrapper();
            transform.Translate(speed * Time.deltaTime * Vector3.forward);
        }

        protected override void Effect(CharacterSheet cs)
        {
            cs.DealDamage(damage); // Apply damage to the target
            gameObject.SetActive(false); // Deactivate the arrow after hitting a target
        }

        protected override IEnumerator PlayAnimation()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}