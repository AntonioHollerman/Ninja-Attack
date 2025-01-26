using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace BaseClasses
{
    public abstract class Weapon : HitBox
    {
        public float AnimationDuration { get; protected set;}
        public List<Sprite> frames;
        public bool deactivateOnStun;
        public float secondsBetweenFrame;
        public GameObject sprite;
        public bool blocksAnimation;

        private SpriteRenderer _sr;
        private Coroutine _animation;
        private Coroutine _execution;
        
        protected abstract IEnumerator Execute();

        public virtual float GetAnimationBlockDuration()
        {
            return frames.Count * secondsBetweenFrame;
        }
        
        public void Attack()
        {
            if (_animation != null)
            {
                StopCoroutine(_animation);
                _animation = null;
            }

            if (_execution != null)
            {
                StopCoroutine(_execution);
                _execution = null;
            }
            Activate(AnimationDuration);
            _animation = StartCoroutine(PlayAnimation());
            _execution = StartCoroutine(Execute());
        }

        protected virtual IEnumerator PlayAnimation()
        {
            int i = 0;
            while (true)
            {
                _sr.sprite = frames[i];
                i++;
                if (i == frames.Count)
                {
                    i = 0;
                }
                yield return new WaitForSeconds(secondsBetweenFrame);
            }
        }

        protected override void AwakeWrapper()
        {
            base.AwakeWrapper();
            _sr = sprite.GetComponent<SpriteRenderer>();
        }
    }
}