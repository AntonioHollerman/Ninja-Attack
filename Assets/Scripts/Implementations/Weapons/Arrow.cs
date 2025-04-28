using BaseClasses;
using UnityEngine;

namespace Implementations.Weapons
{
    public class Arrow : HitBox
    {
        public float atkMultiplier;
        public float speed;
        protected override void Effect(CharacterSheet cs)
        {
            cs.DealDamage(atkMultiplier * parent.Atk, parent);
            Destroy(gameObject);
        }

        protected override void TriggerEnterWrapper(Collider other)
        {
            CharacterSheet cs = other.GetComponent<CharacterSheet>();

            if (cs == null)
            {
                Destroy(gameObject);
            }
        }

        protected override void AwakeWrapper()
        {
            Activate(10);
        }

        protected override void UpdateWrapper()
        {
            transform.Translate(Time.deltaTime * speed * Vector3.forward);
        }
    }
}