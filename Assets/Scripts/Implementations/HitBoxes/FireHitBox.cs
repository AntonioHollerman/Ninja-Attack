using BaseClasses;
using Implementations.Animations;
using Implementations.Extras;
using Implementations.Weapons;
using UnityEngine;

namespace Implementations.HitBoxes
{
    public class FireHitBox : HitBox
    {

        public float duration;
        public bool damageSelf;
        protected override void Effect(CharacterSheet cs)
        {
            float baseDamage = 1.15f * parent.Atk;
            float burnEffectDps = 0.2f * parent.Atk;
            cs.DealDamage(baseDamage, parent);
            cs.LoadEffect(((sheet, deltaTime) => sheet.DealDamage(deltaTime * burnEffectDps, parent)), duration);
        }
        
        protected override void TriggerEnterWrapper(Collider other)
        {
            if (damageSelf)
            {
                ActiveIgnore.Remove(parent);
            }
            base.TriggerEnterWrapper(other);
        }
    }
}