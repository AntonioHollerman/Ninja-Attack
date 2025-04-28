using System;
using System.Collections;
using System.Collections.Generic;
using BaseClasses;
using UnityEngine;

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
        public string attackPath;
        public string walkPath;
        public string idlePath;

        [Header("Animations FPS")] 
        public float hurtFps;
        public float spellCastFps;
        public float attackFps;
        public float walkFps;
        public float idleFps;

        public int AniIndex { private set; get;}
        private readonly Dictionary<AnimationState, AnimationSet> _animations = new();
        private readonly Dictionary<AnimationState, float> _fps = new();

        public static float ForwardToDegrees(Vector3 f)
        {
            float x = Mathf.Clamp(f.x, -1, 1);
            float y = Mathf.Clamp(f.y, -1, 1);
            float result;
            
            if (x != 0)
            {
                result = Mathf.Atan(y / x) * Mathf.Rad2Deg;
                if (x < 0)
                {
                    result += 180f;
                }
            } 
            else
            {
                result = Mathf.Asin(y) * Mathf.Rad2Deg;
            }
            
            if (result < 0)
            {
                result += 360;
            }
            return result;
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

        private float GetSecondsBetweenFrames(AnimationState state)
        {
            return 1 / _fps[state];
        }
        public float GetDuration(AnimationState state)
        {
            return _animations[state].Length * GetSecondsBetweenFrames(state);
        }

        public int GetAnimationLength(AnimationState state)
        {
            return _animations[state].Length;
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
            Sprite[] leftSet  = new ArraySegment<Sprite>(frames,    stepSize,stepSize).ToArray();
            Sprite[] downSet  = new ArraySegment<Sprite>(frames,2 * stepSize,stepSize).ToArray();
            Sprite[] rightSet = new ArraySegment<Sprite>(frames,3 * stepSize,stepSize).ToArray();

            set = new AnimationSet(upSet, downSet, rightSet, leftSet);
            _animations.Add(targetState, set);
        }

        private IEnumerator StartAnimation()
        {
            AniIndex = 0;
            AnimationState lastState = curState;
            while (true)
            {
                if (curState == AnimationState.Walk)
                {
                    curState = parent.rb.velocity == Vector3.zero ? AnimationState.Idle : AnimationState.Walk;
                }
                
                if (lastState != curState)
                {
                    AniIndex = 0;
                    lastState = curState;
                }

                if (AniIndex == _animations[curState].Length)
                {
                    AniIndex = 0;
                    curState = parent.rb.velocity == Vector3.zero ? AnimationState.Idle : AnimationState.Walk;
                }
                Vector3 forward = parent.transform.forward;
                float degrees = ForwardToDegrees(forward);
                Direction dir = DegToDir(degrees);
                
                body.sprite = _animations[curState].GetFrame(dir, AniIndex);
                AniIndex++;
                yield return new WaitForSeconds(GetSecondsBetweenFrames(curState));
                yield return new WaitUntil(() => !parent.IsStunned && !CharacterSheet.UniversalStopCsUpdateLoop);
            }
        } 
        private void Awake()
        {
            _fps[AnimationState.Hurt] = hurtFps;
            _fps[AnimationState.SpellCast] = spellCastFps;
            _fps[AnimationState.Attack] = attackFps;
            _fps[AnimationState.Walk] = walkFps;
            _fps[AnimationState.Idle] = idleFps;
            if (hurtPath != null)
            {
                LoadAnimation(AnimationState.Hurt, hurtPath);
            }

            if (attackPath != null)
            {
                LoadAnimation(AnimationState.Attack, attackPath);
            }

            if (spellCastPath != null)
            {
                LoadAnimation(AnimationState.SpellCast, spellCastPath);
            }

            if (walkPath != null)
            {
                LoadAnimation(AnimationState.Walk, walkPath);
            }

            if (idlePath != null)
            {
                LoadAnimation(AnimationState.Idle, idlePath);
            }
            
            curState = AnimationState.Walk;
            StartCoroutine(StartAnimation());
        }

        private void LateUpdate()
        {
            transform.rotation = Quaternion.identity; // Resets rotation to world space
        }
    }
}