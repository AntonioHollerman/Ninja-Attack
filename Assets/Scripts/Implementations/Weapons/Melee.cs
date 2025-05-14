using System.Collections;
using System.Collections.Generic;
using BaseClasses;
using Implementations.Animations;
using UnityEngine;
using AnimationState = Implementations.Animations.CharacterAnimation.AnimationState;

namespace Implementations.Weapons
{
    // Parent Roc: (-90, 0, 180)
    // Body Roc: (90, 0, 0)
    public class Melee : HitBox
    {
        public SpriteRenderer sprite;
        public bool debugModeOn;
        
        public bool deactivateOnStun;
        public float atkMultiplier;
        public int startHitBoxIndex;
        
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

            StartCoroutine(HitBoxListener());
        }

        private IEnumerator HitBoxListener()
        {
            Collider.enabled = false;
            ActiveIgnore = new List<CharacterSheet>(parent.allies);
            while (true)
            {
                yield return new WaitUntil(() => parent.body.curState == AnimationState.Attack && parent.body.AniIndex < startHitBoxIndex);
                yield return new WaitUntil(() => parent.body.AniIndex >= startHitBoxIndex);
                Collider.enabled = true;
                
                yield return new WaitUntil(() => parent.body.curState != AnimationState.Attack || parent.body.AniIndex < startHitBoxIndex);
                Collider.enabled = false;
                ActiveIgnore = new List<CharacterSheet>(parent.allies);
            }
        }

        protected override void UpdateWrapper()
        {
            base.UpdateWrapper();
            if (Collider.enabled)
            {
                sprite.color = _activeC;
            }
            else
            {
                sprite.color = _nonactiveC;
            }
        }

        protected override void Effect(CharacterSheet cs)
        {
            cs.DealDamage(atkMultiplier * parent.Atk, parent);
        }

        public virtual void Attack()
        {
            parent.body.curState = AnimationState.Attack;
        }

        protected override void TriggerEnterWrapper(Collider other)
        {
            // Attempt to get the CharacterSheet component from the collided object
            CharacterSheet cs = other.gameObject.GetComponent<CharacterSheet>();
            
            // If no CharacterSheet component is found, or if the character is in the ignore list, exit the method
            if (cs == null || ActiveIgnore.Contains(cs))
            {
                return;
            }

            StartCoroutine(SpawnHitMark(other.gameObject));
        }

        private IEnumerator SpawnHitMark(GameObject target)
        {
            GameObject hitMarkPrefab = Resources.Load<GameObject>(CharacterSheet.HitMarkPath);
            GameObject hitMarkGo = Instantiate(
                hitMarkPrefab, 
                target.transform.position, 
                hitMarkPrefab.transform.rotation
                );
            LoopAnimation ani = hitMarkGo.GetComponent<LoopAnimation>();
            ani.StartAnimation();
            
            while (hitMarkGo != null)
            {
                hitMarkGo.transform.position = target.transform.position;
                yield return null;
            }
        }
    }
}