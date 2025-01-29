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
        private float _secondsBetweenFrame;
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
            if (_animation != null)
            {
                StopCoroutine(_animation);
            }
            _animation = StartCoroutine(PlayAnimation());
        }

        private void GetFrames()
        {
            _secondsBetweenFrame = 1 / framesPerSecond;
            
            if (frames.Length == 0)
            {
                frames = Resources.LoadAll<Sprite>(path);
            }
        }
        
        private IEnumerator PlayAnimation()
        {
            int i = 0;
            while (true)
            {
                sr.sprite = frames[i];
                i++;
                if (i == frames.Length && loopOnce)
                {
                    Destroy(gameObject);
                }
                if (i == frames.Length)
                {
                    i = 0;
                }
                yield return new WaitForSeconds(_secondsBetweenFrame);
            }
        }
    }
}