using System;
using System.Collections.Generic;
using UnityEngine;

namespace Implementations.Animations.UniqueAnimation
{
    public class IceShieldAnimation : LoopAnimation
    {
        [Header("Ice Shield Parameters")]
        public string initAnimationPath;
        public float durationLooped;
        public override void GetFrames()
        {
            FrameIndex = 0;
            Sprite[] initAni = new ArraySegment<Sprite>(Resources.LoadAll<Sprite>(initAnimationPath), 0, 9)
                .ToArray();
            Sprite[] loopAni = new ArraySegment<Sprite>(Resources.LoadAll<Sprite>(initAnimationPath), 9, 3)
                .ToArray();
            Sprite[] endAni = new ArraySegment<Sprite>(Resources.LoadAll<Sprite>(initAnimationPath), 12, 4)
                .ToArray();

            float secondsBetweenFrames = 1 / framesPerSecond;
            int framesNeeded = (int)(durationLooped / secondsBetweenFrames) + 1;

            List<Sprite> framesArr = new List<Sprite>(initAni);
            
            int count = 0;
            int index = 0;
            while (count < framesNeeded)
            {
                framesArr.Add(loopAni[index]);
                index++;
                count++;

                if (index >= loopAni.Length)
                {
                    index = 0;
                }
            }
            
            framesArr.AddRange(endAni);

            frames = framesArr.ToArray();
        }
    }
}