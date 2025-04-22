using System.Collections.Generic;
using UnityEngine;

namespace Implementations.Animations.CharacterAnimation
{
    public class AnimationSet
    {
        public int Length{get; private set; }
        private Dictionary<Direction, Sprite[]> _frames = new();
        
        public AnimationSet(Sprite[] upSet, Sprite[] downSet, Sprite[] rightSet, Sprite[] leftSet)
        {
            Length = upSet.Length;
            _frames.Add(Direction.Up, upSet);
            _frames.Add(Direction.Down, downSet);
            _frames.Add(Direction.Left, leftSet);
            _frames.Add(Direction.Right, rightSet);
        }

        public Sprite GetFrame(Direction dir, int index)
        {
            return _frames[dir][index];
        }
    }
}