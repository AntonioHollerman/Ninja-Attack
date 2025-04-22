using System;
using System.Collections;
using System.Collections.Generic;
using BaseClasses;
using UnityEngine;
using UnityEngine.Serialization;

namespace Implementations.Animations.CharacterAnimation
{
    public class BodyAnimation : MonoBehaviour
    {
        [Header("Target")] 
        public CharacterSheet parent;
        public SpriteRenderer body;
        public AnimationState curState;

        [Header("Animations")]
        public string hurtPath;
        public string spellCastPath;
        public string slashPath;
        public string walkPath;
        public float fps;

        private Dictionary<AnimationState, AnimationSet> _animations;
        private float _secondsBetweenFrames;

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

        private void LoadAnimation(AnimationState targetState, string path)
        {
            Sprite[] frames = Resources.LoadAll<Sprite>(path);
            AnimationSet set;
            if (targetState == AnimationState.Hurt)
            {
                set = new AnimationSet(frames, frames, frames, frames);
                _animations.Add(targetState, set);
                return;
            }
            
            int stepSize = frames.Length / 4;
            Sprite[] upSet    = new ArraySegment<Sprite>(frames,           0,stepSize).ToArray();
            Sprite[] downSet  = new ArraySegment<Sprite>(frames,    stepSize,stepSize).ToArray();
            Sprite[] rightSet = new ArraySegment<Sprite>(frames,2 * stepSize,stepSize).ToArray();
            Sprite[] leftSet  = new ArraySegment<Sprite>(frames,3 * stepSize,stepSize).ToArray();

            set = new AnimationSet(upSet, downSet, rightSet, leftSet);
            _animations.Add(curState, set);
        }

        private IEnumerator StartAnimation()
        {
            int index = 0;
            AnimationState lastState = curState;
            while (true)
            {
                if (lastState != curState)
                {
                    index = 0;
                    lastState = curState;
                }

                if (index == _animations[curState].Length)
                {
                    index = 0;
                }
                Vector3 forward = parent.transform.forward;
                float degrees = ForwardToDegrees(forward);
                Direction dir = DegToDir(degrees);
                
                body.sprite = _animations[curState].GetFrame(dir, index);
                index++;
                yield return new WaitForSeconds(_secondsBetweenFrames);
                yield return new WaitUntil(() => !parent.IsStunned && !CharacterSheet.UniversalStopCsUpdateLoop);
            }
        } 
        private void Awake()
        {
            if (hurtPath != null)
            {
                LoadAnimation(AnimationState.Hurt, hurtPath);
            }

            if (slashPath != null)
            {
                LoadAnimation(AnimationState.Melee, slashPath);
            }

            if (spellCastPath != null)
            {
                LoadAnimation(AnimationState.SpellCast, spellCastPath);
            }

            if (walkPath != null)
            {
                LoadAnimation(AnimationState.Walk, spellCastPath);
            }

            _secondsBetweenFrames = 1 / fps;
            curState = AnimationState.Walk;
            StartCoroutine(StartAnimation());
        }

        private void LateUpdate()
        {
            transform.rotation = Quaternion.identity; // Resets rotation to world space
        }
    }
}