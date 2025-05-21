using System.Collections.Generic;
using UnityEngine;

namespace Implementations.Animations.UniqueAnimation
{
    public class IceShieldAnimation : LoopAnimation
    {
        [Header("Ice Shield Parameters")]
        public string initAnimationPath;
        public string loopAnimationPath;
        public float durationLooped;
        public override void GetFrames()
        {
            Sprite[] initAni = Resources.LoadAll<Sprite>(initAnimationPath);
            Sprite[] loopAni = Resources.LoadAll<Sprite>(loopAnimationPath);

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

            frames = framesArr.ToArray();
        }
    }
}