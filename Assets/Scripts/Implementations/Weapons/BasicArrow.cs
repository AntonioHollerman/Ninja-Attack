using System.Collections;
using System.Collections.Generic;
using BaseClasses;
using Implementations.Extras;
using UnityEngine;

namespace Implementations.Weapons
{
    public class BasicArrow : Weapon
    {
        public int damage;
        public float speed = 5f; // Speed of the arrow
        public float maxTravelDistance = 15f; // Maximum distance the arrow can travel before deactivating

        protected override void AwakeWrapper()
        {
            base.AwakeWrapper();
            
            AnimationDuration = speed != 0 ? maxTravelDistance / speed : 3; // Calculate duration based on speed and distance
        }

        protected override void Effect(CharacterSheet cs)
        {
            Vector3 pos = cs.gameObject.transform.position;
            GameObject prefab = Resources.Load<GameObject>(CharacterSheet.HitMarkPath);
            LoopAnimation script = Instantiate(prefab, pos, prefab.transform.rotation).GetComponent<LoopAnimation>();
            
            Deactivate();
            script.StartAnimation();
            cs.DealDamage(damage); // Apply damage to the target
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