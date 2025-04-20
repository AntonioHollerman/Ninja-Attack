using System.Collections;
using BaseClasses;
using UnityEngine;

namespace Implementations.Weapons
{
    public class Melee : HitBox
    {
        public SpriteRenderer sprite;
        public bool debugModeOn;
        
        public bool deactivateOnStun;
        public float animationBlockDuration;
        public float atkMultiplier;
        public float zOffset;

        public bool stopAttack;
        private readonly Color _activeC = new Color(0.1921568f, 0.792156f, 0);
        private readonly Color _nonactiveC = new Color(1, 0, 0);

        protected override void AwakeWrapper()
        {
            base.AwakeWrapper();
            if (debugModeOn)
            {
                sprite.enabled = true;
                sprite.color = _nonactiveC;
            }
            else
            {
                sprite.enabled = false;
            }

            stopAttack = false;
        }

        protected override void Effect(CharacterSheet cs)
        {
            cs.DealDamage(atkMultiplier * parent.Atk, parent);
        }

        public virtual bool Attack()
        {
            StartCoroutine(HitBoxListener());
            return true;
        }

        private IEnumerator HitBoxListener()
        {
            Activate(animationBlockDuration);
            
            sprite.color = _activeC;
            yield return new WaitUntil(() => stopAttack || !Collider.enabled);
            sprite.color = _nonactiveC;
        }
    }
}