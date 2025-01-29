using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Implementations.Extras
{
    public class MultiSpriteAnimation : MonoBehaviour
    {
        public List<LoopAnimation> spriteAnimations;
        public bool loopOnce;
        public bool playAsynchronously;
        
        private Coroutine _animation;
        private int _numOfSprite;

        private IEnumerator LinearAnimation()
        {
            while (true)
            {
                int i = 0;
                foreach (LoopAnimation sprite in spriteAnimations)
                {
                    if (i >= _numOfSprite)
                    {
                        break;
                    }
                    sprite.gameObject.SetActive(true);
                    sprite.StartAnimation();
                    yield return new WaitForSeconds(GetDuration(sprite));
                    sprite.gameObject.SetActive(false);
                    i++;
                }

                if (loopOnce)
                {
                    Destroy(gameObject);
                }
            }
        }

        private IEnumerator AsynchronousAnimation()
        {
            int i = 0;
            foreach (LoopAnimation sprite in spriteAnimations)
            {
                if (i >= _numOfSprite)
                {
                    break;
                }
                sprite.gameObject.SetActive(true);
                sprite.StartAnimation();
                i++;
            }

            yield return new WaitForSeconds(GetLongestDuration());
            if (loopOnce)
            {
                Destroy(gameObject);
            }
        }

        private float GetDuration(LoopAnimation ani)
        {
            return ani.frames.Length * (1 / ani.framesPerSecond);
        }

        private float GetLongestDuration()
        {
            float longest = 0;
            foreach (var sprite in spriteAnimations)
            {
                float dur = GetDuration(sprite);
                if (dur > longest)
                {
                    longest = dur;
                }
            }
            return longest;
        }
        public void StartAnimation(int numOfSprite)
        {
            _numOfSprite = numOfSprite;
            if (_animation != null)
            {
                StopCoroutine(_animation);
            }

            if (playAsynchronously)
            {
                _animation  = StartCoroutine(AsynchronousAnimation());
            }
            else
            {
                _animation = StartCoroutine(LinearAnimation());
            }
        }
    }
}