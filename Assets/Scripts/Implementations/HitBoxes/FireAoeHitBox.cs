using System.Collections.Generic;
using BaseClasses;
using UnityEngine;

namespace Implementations.HitBoxes
{
    public class FireAoeHitBox : HitBox
    {
        public bool damageSelf;
        private List<CharacterSheet> _ignore;
        protected override void Effect(CharacterSheet cs)
        {
            
        }

        protected override void TriggerEnterWrapper(Collider other)
        {
            if (damageSelf)
            {
                ActiveIgnore.Remove(parent);
            }
            _ignore = new List<CharacterSheet>(ActiveIgnore);
        }

        protected override void TriggerStayWrapper(Collider other)
        {
            float burnEffectDps = 0.4f * parent.Atk;
            CharacterSheet cs = other.gameObject.GetComponent<CharacterSheet>();
            if (cs == null || _ignore.Contains(cs))
            {
                return;
            }
            cs.DealDamage(burnEffectDps * Time.deltaTime, parent);
        }
    }
}