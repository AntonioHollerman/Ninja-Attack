using System.Collections;
using System.Collections.Generic;
using BaseClasses;
using Implementations.Animations;
using Implementations.HitBoxes;
using Unity.VisualScripting;
using UnityEngine;

namespace Implementations.Techniques
{
    public class FireSummon : Technique
    {
        public float detectDistance = 20;
        public int frameStartHitBox;
        public GameObject summonPrefab;
        protected override void Execute()
        {
            GameObject target;
            if (parent is Player)
            {
                target = GetClosestTarget(Hostile.Hostiles);
            } else 
            {
                target = GameObject.Find("SoloPlayer");
            }
            
            GameObject techGo = Instantiate(
                summonPrefab, 
                target.transform.position, 
                summonPrefab.transform.rotation
                );
            LoopAnimation animationScript = techGo.GetComponent<LoopAnimation>();
            FireHitBox hitBoxScript = techGo.GetComponent<FireHitBox>();
            hitBoxScript.parent = parent;

            StartCoroutine(FramesListener(animationScript, hitBoxScript));
            animationScript.StartAnimation();
        }
        
        private IEnumerator FramesListener(LoopAnimation ani, FireHitBox hitBox)
        {
            yield return new WaitUntil(() => ani.FrameIndex >= frameStartHitBox);
            int framesLeft = 3;
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
        
        public override TechEnum GetTechEnum()
        {
            return TechEnum.FireSummon;
        }
    }
}