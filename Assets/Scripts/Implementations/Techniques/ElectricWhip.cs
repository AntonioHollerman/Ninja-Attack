using System.Collections;
using BaseClasses;
using Implementations.Animations;
using Implementations.HitBoxes;
using UnityEngine;

namespace Implementations.Techniques
{
    public class ElectricWhip : Technique
    {
        [Header("Electric Whip Components")]
        public GameObject meleeAnimationPrefab;
        public float forwardOffset;
        public int frameStartHitBox;
        
        protected override void Execute()
        {
            GameObject techGo = Instantiate(meleeAnimationPrefab);
            LoopAnimation animationScript = techGo.GetComponent<LoopAnimation>();
            ElectricHitBox hitBoxScript = techGo.GetComponent<ElectricHitBox>();
            hitBoxScript.parentTech = this;
            hitBoxScript.parent = parent;

            StartCoroutine(TrackParent(animationScript));
            StartCoroutine(FramesListener(animationScript, hitBoxScript));
            animationScript.StartAnimation();
        }
        
        private IEnumerator FramesListener(LoopAnimation ani, ElectricHitBox hitBox)
        {
            yield return new WaitUntil(() => ani.FrameIndex >= frameStartHitBox);
            int framesLeft = 3;
            hitBox.Activate(framesLeft * ani.SecondsBetweenFrame);
        }
        
        private IEnumerator TrackParent(LoopAnimation ani)
        {
            ani.transform.rotation = parent.pTransform.rotation;
            while (true)
            {
                if (ani == null)
                {
                    break;
                }

                ani.transform.position = parent.pTransform.position;
                ani.transform.rotation = parent.pTransform.rotation;
                ani.transform.Translate(Vector3.forward * forwardOffset);

                ani.transform.rotation = Quaternion.LookRotation(parent.pTransform.forward, Vector3.forward);

                yield return null;
            }
        }

        protected override float GetSpellCastDuration()
        {
            return meleeAnimationPrefab.GetComponent<LoopAnimation>().GetAnimationDuration();
        }
    }
}