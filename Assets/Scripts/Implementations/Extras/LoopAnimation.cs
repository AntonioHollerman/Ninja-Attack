using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

<<<<<<<< HEAD:Assets/Scripts/Implementations/AnimationScripts/LoopAnimation.cs
namespace Implementations.AnimationScripts
========
namespace Implementations.Extras
>>>>>>>> 3d8c143dab44dd7d58607a49bf171b162714fa96:Assets/Scripts/Implementations/Extras/LoopAnimation.cs
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
<<<<<<<< HEAD:Assets/Scripts/Implementations/AnimationScripts/LoopAnimation.cs
           GetFrames();
========
            GetFrames();
>>>>>>>> 3d8c143dab44dd7d58607a49bf171b162714fa96:Assets/Scripts/Implementations/Extras/LoopAnimation.cs
        }

        private void OnEnable()
        {
            GetFrames();
<<<<<<<< HEAD:Assets/Scripts/Implementations/AnimationScripts/LoopAnimation.cs
========
        }

        public void StartAnimation()
        {
            if (_animation != null)
            {
                StopCoroutine(_animation);
            }
            _animation = StartCoroutine(PlayAnimation());
>>>>>>>> 3d8c143dab44dd7d58607a49bf171b162714fa96:Assets/Scripts/Implementations/Extras/LoopAnimation.cs
        }

        private void GetFrames()
        {
            _secondsBetweenFrame = 1 / framesPerSecond;
            if (path.Equals("") || path == null)
            {
                return;
            }
            frames = Resources.LoadAll<Sprite>(path);
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
    }
}