using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Implementations.Animations.UniqueAnimation
{
    public class FireRainAnimation : LoopAnimation
    {
        public float secondsLooped;
        
        public override void GetFrames()
        {
            FrameIndex = 0;
            Sprite[] aniFrames = Resources.LoadAll<Sprite>(path);
            
            Sprite[] part1 = new ArraySegment<Sprite>(aniFrames, 
                    0,
                    7)
                .ToArray();
            
            List<Sprite> part2 = new List<Sprite>();
            Sprite[] toRepeat = new ArraySegment<Sprite>(aniFrames,
                8,
                6)
                .ToArray();

            int index = 0;
            int neededFrames = (int) (secondsLooped / SecondsBetweenFrame);
            for (int i = 0; i < neededFrames; i++)
            {
                part2.Add(toRepeat[index]);
                index++;
                if (index == toRepeat.Length)
                {
                    index = 0;
                }
            }
            
            Sprite[] part3 = new ArraySegment<Sprite>(aniFrames, 
                    14,
                    10)
                .ToArray();
            
            List<Sprite> tempFrames = new List<Sprite>(part1);
            tempFrames.AddRange(part2);
            tempFrames.AddRange(part3);

            frames = tempFrames.ToArray();
        }
    }
}