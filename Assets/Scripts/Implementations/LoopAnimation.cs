using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Implementations
{
    public class LoopAnimation : MonoBehaviour
    {
        public SpriteRenderer sr;
        public float secondsBetweenFrame;
        public List<Sprite> frames;
        public bool loopOnce;

        private void Start()
        {
            StartCoroutine(PlayAnimation());
        }

        private IEnumerator PlayAnimation()
        {
            int i = 0;
            while (true)
            {
                sr.sprite = frames[i];
                i++;
                if (i == frames.Count && loopOnce)
                {
                    Destroy(gameObject);
                }
                if (i == frames.Count)
                {
                    i = 0;
                }
                yield return new WaitForSeconds(secondsBetweenFrame);
            }
        }
    }
}