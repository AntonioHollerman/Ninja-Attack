using System;
using UnityEngine;

namespace Implementations.Animations.UniqueAnimation
{
    public class ShockWave : LoopAnimation
    {
        public float growthRate;
        public override void GetFrames()
        {
            FrameIndex = 0;
            frames = new ArraySegment<Sprite>(Resources.LoadAll<Sprite>(path), 4, 6).ToArray();
        }

        protected override void UpdateWrapper()
        {
            base.UpdateWrapper();
            if (FrameIndex == 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }

            transform.localScale *= Mathf.Pow(growthRate, Time.deltaTime);
            if (FrameIndex == frames.Length - 1)
            {
                Debug.Log(frames.Length * SecondsBetweenFrame);
                Debug.Log(Mathf.Pow(growthRate, frames.Length * SecondsBetweenFrame));
            }
        }
    }
}