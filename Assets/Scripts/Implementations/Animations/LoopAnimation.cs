﻿using System;
using System.Collections;
using UnityEngine;

namespace Implementations.Animations
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
        public bool startAtRuntime;
        private Coroutine _animation;

        private void Start()
        {
            SecondsBetweenFrame = 1 / framesPerSecond;
            GetFrames();
        }

        private void Awake()
        { 
           SecondsBetweenFrame = 1 / framesPerSecond;
           GetFrames();
           if (startAtRuntime)
           {
               StartAnimation();
           }
        }

        private void OnEnable()
        {
            SecondsBetweenFrame = 1 / framesPerSecond;
            GetFrames();
        }

        public void StopAnimation()
        {
            if (_animation != null)
            {
                StopCoroutine(_animation);
            }
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

        public virtual void GetFrames()
        {
            FrameIndex = 0;
            
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
                if (FrameIndex >= frames.Length)
                {
                    FrameIndex = 0;
                }
                yield return new WaitForSeconds(SecondsBetweenFrame);
            }
        }

        public float GetAnimationDuration()
        {
            GetFrames();
            SecondsBetweenFrame = 1 / framesPerSecond;
            
            Debug.Log(frames.Length);
            Debug.Log(SecondsBetweenFrame);
            return frames.Length * SecondsBetweenFrame;
        }

        protected virtual void UpdateWrapper()
        {
            
        }

        private void Update()
        {
            UpdateWrapper();
        }
    }
}