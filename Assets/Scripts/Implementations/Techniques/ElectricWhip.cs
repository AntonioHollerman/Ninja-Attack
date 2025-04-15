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
        public float yOffset;
        public int frameStartHitBox;
        
        public override void Execute()
        {
            
        }
        
        private IEnumerator FramesListener(LoopAnimation ani, ElectricHitBox hitBox)
        {
            yield return new WaitUntil(() => ani.FrameIndex >= frameStartHitBox);
            int framesLeft = ani.frames.Length - ani.FrameIndex;
            hitBox.Activate(framesLeft * ani.SecondsBetweenFrame);
        }
        
        private IEnumerator TrackParent(LoopAnimation ani)
        {
            ani.transform.rotation = parent.transform.rotation;
            while (true)
            {
                if (ani == null)
                {
                    break;
                }

                ani.transform.position = parent.transform.position;
                ani.transform.Translate(Vector3.forward * yOffset);

                yield return null;
            }
        }
    }
}