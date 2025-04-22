using System;
using System.Collections.Generic;
using UnityEngine;

namespace Implementations.Animations.CharacterAnimation
{
    public class BodyAnimation : MonoBehaviour
    {
        public SpriteRenderer body;

        public string hurtPath;
        public string spellCastPath;
        public string slashPath;
        public string walkPath;

        private Dictionary<AnimationState, AnimationSet> _animations;

        public static float ForwardToDegrees(Vector3 f)
        {
            float x = Mathf.Clamp(f.x, -1, 1);
            float y = Mathf.Clamp(f.y, -1, 1);

            if (x != 0)
            {
                float result = Mathf.Atan(y / x) * Mathf.Rad2Deg;
                if (x < 0)
                {
                    result += 180f;
                }

                if (result < 0)
                {
                    result += 360;
                }

                return result;
            }
            else
            {
                float result = Mathf.Asin(y) * Mathf.Rad2Deg;
                return result;
            }
        }

        public static Direction DegToDir(float degrees)
        {
            if (120 >= degrees && degrees >= 60)
            {
                return Direction.Up;
            }

            if (240 > degrees && degrees > 120)
            {
                return Direction.Left;
            }

            if (300 >= degrees && degrees >= 240)
            {
                return Direction.Down;
            }
            
            return Direction.Right;
        }

        private void LoadAnimation(AnimationState state, string path)
        {
            Sprite[] frames = Resources.LoadAll<Sprite>(path);
            AnimationSet set;
            if (state == AnimationState.Hurt)
            {
                set = new AnimationSet(frames, frames, frames, frames);
                _animations.Add(state, set);
                return;
            }
            
            int stepSize = frames.Length / 4;
            Sprite[] upSet    = new ArraySegment<Sprite>(frames,           0,stepSize).ToArray();
            Sprite[] downSet  = new ArraySegment<Sprite>(frames,    stepSize,stepSize).ToArray();
            Sprite[] rightSet = new ArraySegment<Sprite>(frames,2 * stepSize,stepSize).ToArray();
            Sprite[] leftSet  = new ArraySegment<Sprite>(frames,3 * stepSize,stepSize).ToArray();

            set = new AnimationSet(upSet, downSet, rightSet, leftSet);
            _animations.Add(state, set);
        }
        private void LoadHurtAnimation()
        {
            
        }

        private void LoadSpellCastAnimation()
        {
            
        }

        private void LoadSlashAnimation()
        {
            
        }

        private void LoadWalkAnimation()
        {
            
        }

        private void Awake()
        {
            if (hurtPath != null)
            {
                LoadHurtAnimation();
            }

            if (slashPath != null)
            {
                LoadSlashAnimation();
            }

            if (spellCastPath != null)
            {
                LoadSpellCastAnimation();
            }

            if (walkPath != null)
            {
                LoadWalkAnimation();
            }
        }

        private void LateUpdate()
        {
            transform.rotation = Quaternion.identity; // Resets rotation to world space
        }
    }
}