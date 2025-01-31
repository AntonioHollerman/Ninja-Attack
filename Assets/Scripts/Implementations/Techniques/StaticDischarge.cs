using System.Collections;
using BaseClasses;
using Implementations.Extras;
using Implementations.Extras.UniqueAnimation;
using Implementations.HitBoxes;
using UnityEngine;

namespace Implementations.Techniques
{
    public class StaticDischarge : Technique
    {
        public GameObject dischargeAnimationPrefab;
        public int frameStartHitBox;
        public override void Execute()
        {
            GameObject techGo = Instantiate(dischargeAnimationPrefab);
            StaticDischargeAnimation animationScript = techGo.GetComponent<StaticDischargeAnimation>();
            StaticDischargeHitBox hitBoxScript = techGo.GetComponent<StaticDischargeHitBox>();
            hitBoxScript.parent = cs;
            
            
            StartCoroutine(TrackParent(animationScript));
            StartCoroutine(FramesListener(animationScript, hitBoxScript));
            animationScript.StartAnimation();
        }

        private IEnumerator FramesListener(LoopAnimation ani, StaticDischargeHitBox hitBox)
        {
            yield return new WaitUntil(() => ani.FrameIndex >= frameStartHitBox);
            int framesLeft = ani.frames.Length - ani.FrameIndex;
            hitBox.Activate(framesLeft * ani.SecondsBetweenFrame);
        }
        private IEnumerator TrackParent(LoopAnimation ani)
        {
            while (true)
            {
                if (ani == null)
                {
                    break;
                }
                ani.transform.position = cs.transform.position;
                yield return null;
            }
        }

        protected override void StartWrapper()
        {
            base.StartWrapper();
            LoopAnimation animationScript = Instantiate(dischargeAnimationPrefab).GetComponent<LoopAnimation>();
            animationBlockDuration = animationScript.GetAnimationDuration();
            Destroy(animationScript.gameObject);
        }
    }
}
