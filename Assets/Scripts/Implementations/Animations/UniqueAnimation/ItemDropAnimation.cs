using System;
using System.Collections.Generic;
using UnityEngine;

namespace Implementations.Animations.UniqueAnimation
{
    public class ItemDropAnimation : LoopAnimation
    {
        public override void GetFrames()
        {
            base.GetFrames();
            Sprite[] spriteArr = Resources.LoadAll<Sprite>(path);
            List<Sprite> frameList = new (new ArraySegment<Sprite>(spriteArr, 2, 2).ToArray());
            for (int i = 3; i > 1; i--)
            {
                frameList.Add(spriteArr[i]);
            }

            frames = frameList.ToArray();
        }
    }
}