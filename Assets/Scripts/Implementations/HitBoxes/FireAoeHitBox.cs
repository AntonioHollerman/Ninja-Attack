using BaseClasses;
using UnityEngine;

namespace Implementations.HitBoxes
{
    public class FireAoeHitBox : HitBox
    {
        public bool damageSelf;
        protected override void Effect(CharacterSheet cs)
        {
            if (damageSelf)
            {
                ActiveIgnore.Remove(parent);
            }
        }

        protected override void TriggerStayWrapper(Collider other)
        {
            float burnEffectDps = 0.2f * parent.Atk;
            CharacterSheet cs = other.gameObject.GetComponent<CharacterSheet>();
            if (cs == null || ActiveIgnore.Contains(cs))
            {
                return;
            }
            cs.DealDamage(burnEffectDps * Time.deltaTime, parent);
        }
    }
}