using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Implementations
{
    public class LoopAnimation : MonoBehaviour
    {
        public SpriteRenderer sr;
        public string path;
        public Sprite[] frames;
        public float framesPerSecond;
        public bool loopOnce;
        private float _secondsBetweenFrame;
        private Coroutine _animation;

        private void Start()
        {
            _animation = StartCoroutine(PlayAnimation());
        }

        private void Awake()
        {
            _secondsBetweenFrame = 1 / framesPerSecond;
            if (!path.Equals(""))
            {
                frames = Resources.LoadAll<Sprite>(path);
            }
        }

        private void OnEnable()
        {
            if (_animation != null)
            {
                StopCoroutine(_animation);
            }

            _secondsBetweenFrame = 1 / framesPerSecond;
            _animation = StartCoroutine(PlayAnimation());
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