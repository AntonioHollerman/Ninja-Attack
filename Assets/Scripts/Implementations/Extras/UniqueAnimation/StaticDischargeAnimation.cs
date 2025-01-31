using System.Collections;
using UnityEngine;

namespace Implementations.Extras.UniqueAnimation
{
    public class StaticDischargeAnimation : LoopAnimation
    {
        public int startToGrow;
        public float growRate;
        protected override IEnumerator PlayAnimation()
        {
            FrameIndex = 0;
            bool growing = false;
            while (true)
            {
                sr.sprite = frames[FrameIndex];
                FrameIndex++;
                if (FrameIndex == frames.Length && loopOnce)
                {
                    Destroy(gameObject);
                }

                if (FrameIndex == startToGrow)
                {
                    growing = true;
                }
                if (growing)
                {
                    transform.localScale *= growRate;
                }
                if (FrameIndex == frames.Length)
                {
                    FrameIndex = 0;
                    transform.localScale = new Vector3(1, 1, 1);
                    growing = false;
                }
                yield return new WaitForSeconds(SecondsBetweenFrame);
            }
        }
    }
}