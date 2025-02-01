using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Implementations.Extras
{
    public class LoopAnimation : MonoBehaviour
    {
        public SpriteRenderer sr;
        public string path;
        public Sprite[] frames;
        public float framesPerSecond = 30;
        public bool loopOnce;
        
        public int FrameIndex { protected set; get;}
        public float SecondsBetweenFrame { private set; get;}
        private Coroutine _animation;

        private void Start()
        {
            GetFrames();
        }

        private void Awake()
        {
           GetFrames();
        }

        private void OnEnable()
        {
            GetFrames();
        }

        public void StartAnimation()
        {
            gameObject.SetActive(true);
            if (_animation != null)
            {
                StopCoroutine(_animation);
            }
            _animation = StartCoroutine(PlayAnimation());
        }

        public void GetFrames()
        {
            FrameIndex = 0;
            SecondsBetweenFrame = 1 / framesPerSecond;
            
            if (frames.Length == 0)
            {
                frames = Resources.LoadAll<Sprite>(path);
            }
        }
        
        protected virtual IEnumerator PlayAnimation()
        {
            FrameIndex = 0;
            while (true)
            {
                sr.sprite = frames[FrameIndex];
                FrameIndex++;
                if (FrameIndex == frames.Length && loopOnce)
                {
                    Destroy(gameObject);
                }
                if (FrameIndex == frames.Length)
                {
                    FrameIndex = 0;
                }
                yield return new WaitForSeconds(SecondsBetweenFrame);
            }
        }

        public float GetAnimationDuration()
        {
            GetFrames();
            return frames.Length * SecondsBetweenFrame;
        }

        public float GetDuration()
        {
            return frames.Length * SecondsBetweenFrame;
        }
    }
}