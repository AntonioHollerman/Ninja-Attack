using System.Collections;
using System.Collections.Generic;
using BaseClasses;
using Implementations.Animations;
using Implementations.HitBoxes;
using UnityEngine;

namespace Implementations.Techniques
{
    public class FireSummon : Technique
    {
        public float detectDistance = 20;
        public int frameStartHitBox;
        public GameObject summonPrefab;

        // Add an AudioClip variable to hold the sound effect
        public AudioClip summonSound;

        protected override void Execute()
        {
            GameObject target;
            if (parent is Player)
            {
                target = GetClosestTarget(Hostile.Hostiles);
            }
            else
            {
                target = GameObject.Find("SoloPlayer");
            }

            GameObject techGo = Instantiate(
                summonPrefab,
                target.transform.position,
                summonPrefab.transform.rotation
            );

            LoopAnimation animationScript = techGo.GetComponent<LoopAnimation>();
            FireHitBox hitBoxScript = techGo.GetComponent<FireHitBox>();
            hitBoxScript.parent = parent;

            // Get the AudioSource component and play the summon sound
            AudioSource audioSource = techGo.GetComponent<AudioSource>();
            if (audioSource != null && summonSound != null)
            {
                audioSource.clip = summonSound;
                audioSource.Play();
            }

            StartCoroutine(FramesListener(animationScript, hitBoxScript));
            animationScript.StartAnimation();
        }

        private IEnumerator FramesListener(LoopAnimation ani, FireHitBox hitBox)
        {
            yield return new WaitUntil(() => ani.FrameIndex >= frameStartHitBox);
            int framesLeft = 3;
            hitBox.Activate(framesLeft * ani.SecondsBetweenFrame);
        }

        private GameObject GetClosestTarget<T>(List<T> targets) where T : CharacterSheet
        {
            GameObject target = null;
            float distance = detectDistance;

            foreach (T t in targets)
            {
                float d = Mathf.Abs((transform.position - t.pTransform.position).magnitude);
                if (d < distance)
                {
                    distance = d;
                    target = t.gameObject;
                }
            }
            return target;
        }
    }
}
