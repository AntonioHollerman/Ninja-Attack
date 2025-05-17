using System.Collections;
using System.Collections.Generic;
using BaseClasses;
using Implementations.Animations;
using Implementations.Animations.UniqueAnimation;
using Implementations.HitBoxes;
using UnityEngine;
using AnimationState = Implementations.Animations.CharacterAnimation.AnimationState;

namespace Implementations.Techniques
{
    public class FireRain : Technique
    {
        public GameObject fireRainPrefab;
        public float detectDistance = 20;
        public int frameStartHitBox;
        public float secondsLooped;
        public float spellCastFps = 12f;
        protected override void Execute()
        {
            GameObject target;
            if (parent is Player)
            {
                target = GetClosestTarget(Hostile.Hostiles);
            } else 
            {
                target = GetClosestTarget(Player.Players); 
            }
            
            GameObject techGo = Instantiate(
                fireRainPrefab, 
                target.transform.position + Vector3.forward, 
                fireRainPrefab.transform.rotation
            );
            FireRainAnimation animationScript = techGo.GetComponent<FireRainAnimation>();
            LingeringFireHitBox hitBoxScript = techGo.GetComponent<LingeringFireHitBox>();

            animationScript.secondsLooped = secondsLooped;
            hitBoxScript.parent = parent;

            StartCoroutine(FramesListener(animationScript, hitBoxScript));
            animationScript.GetFrames();
            animationScript.StartAnimation();
        }
        
        private IEnumerator FramesListener(FireRainAnimation ani, LingeringFireHitBox hitBox)
        {
            yield return new WaitUntil(() => ani.FrameIndex >= frameStartHitBox);
            int framesLeft = ani.frames.Length - 17;
            hitBox.Activate(framesLeft * ani.SecondsBetweenFrame);
        }

        private GameObject GetClosestTarget<T>(List<T> targets) where T : CharacterSheet
        {
            GameObject target = null;
            float distance = detectDistance;

            foreach (T t in targets)
            {
                float d = Mathf.Abs((transform.position - t.pTransform.position).magnitude);
                if (d < distance)
                {
                    distance = d;
                    target = t.gameObject;
                }
            }
            return target;
        }

        protected override float GetSpellCastDuration()
        {
            return parent.body.GetAnimationLength(AnimationState.SpellCast) * spellCastFps;
        }
    }
}