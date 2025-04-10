using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Implementations.Animations.UniqueAnimation
{
    public class FireSummonAnimation : LoopAnimation
    {
        public string vortexPath;
        public string hurricanePath;
        public float yDisplacement;
        public override void GetFrames()
        {
            FrameIndex = 0;
            
            Sprite[] vortexSpliced = new ArraySegment<Sprite>(Resources.LoadAll<Sprite>(vortexPath), 
                0,
                20)
                .ToArray();
            Sprite[] hurricaneSpliced = new ArraySegment<Sprite>(Resources.LoadAll<Sprite>(hurricanePath),
                6, 
                10)
                .ToArray();
            
            List<Sprite> tempFrames = new List<Sprite>(vortexSpliced);
            tempFrames.AddRange(hurricaneSpliced);

            frames = tempFrames.ToArray();
        }

        protected override IEnumerator PlayAnimation()
        {
            FrameIndex = 0;
            while (true)
            {
                sr.sprite = frames[FrameIndex];
                FrameIndex++;
                if (FrameIndex == frames.Length && loopOnce)
                {
                    Destroy(gameObject);
                }
                if (FrameIndex == frames.Length)
                {
                    FrameIndex = 0;
                    transform.Translate(Vector3.up * yDisplacement);
                }

                if (FrameIndex == 20)
                {
                    transform.Translate(Vector3.down * yDisplacement);
                }
                yield return new WaitForSeconds(SecondsBetweenFrame);
            }
        }
    }
}