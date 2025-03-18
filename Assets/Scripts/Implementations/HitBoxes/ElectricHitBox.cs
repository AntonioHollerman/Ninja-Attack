using System;
using System.Collections;
using BaseClasses;
using Implementations.Extras;
using UnityEngine;

namespace Implementations.HitBoxes
{
    public class ElectricHitBox : HitBox
    {
        public Technique parentTech;
        public float stunDuration;
        public GameObject hitPrefab;
        public GameObject stunPrefab;
        
        protected override void Effect(CharacterSheet cs)
        {
            cs.DealDamage(1.35f * parent.Atk, parent);
            cs.Stun(stunDuration);
            parentTech.StartCoroutine(HitAnimation(cs));
        }
        
        private IEnumerator TrackTarget(LoopAnimation ani, CharacterSheet cs)
        {
            void UpdatePosition()
            {
                try
                {
                    ani.transform.position = cs.transform.position;
                }
                catch (Exception e)
                {
                    // Ignore
                }
            }
            ani.StartAnimation();
            while (true)
            {
                if (ani == null)
                {
                    break;
                }
                UpdatePosition();
                yield return null;
            }
        }

        private IEnumerator HitAnimation(CharacterSheet cs)
        {
            LoopAnimation hitAni = Instantiate(hitPrefab).GetComponent<LoopAnimation>();
            parentTech.StartCoroutine(TrackTarget(hitAni, cs));
            yield return new WaitForSeconds(hitAni.frames.Length * hitAni.SecondsBetweenFrame);
            if (hitAni != null)
            {
                Destroy(hitAni.gameObject);
            }

            GameObject stunObj = Instantiate(stunPrefab);
            if (cs.isLarge)
            {
                stunObj.transform.localScale *= 5;
            }
            
            LoopAnimation stunAni = stunObj.GetComponent<LoopAnimation>();
            parentTech.StartCoroutine(TrackTarget(stunAni, cs));
            yield return new WaitUntil(() => cs == null || !cs.IsStunned);
            Destroy(stunAni.gameObject);
        }
    }
}