using System.Collections;
using System.Collections.Generic;
using BaseClasses;
using Implementations.Animations;
using Implementations.Animations.UniqueAnimation;
using Implementations.HitBoxes;
using UnityEngine;
using AnimationState = Implementations.Animations.CharacterAnimation.AnimationState;

namespace Implementations.Techniques
{
    public class FireRain : Technique
    {
        public GameObject fireRainPrefab;
        public float detectDistance = 20;
        public int frameStartHitBox;
        public float secondsLooped;

        // Add an AudioClip variable to hold the sound effect
        public AudioClip attackSound;

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
                fireRainPrefab,
                target.transform.position + Vector3.up * 0.2f,
                fireRainPrefab.transform.rotation
            );

            FireRainAnimation animationScript = techGo.GetComponent<FireRainAnimation>();
            LingeringFireHitBox hitBoxScript = techGo.GetComponent<LingeringFireHitBox>();

            animationScript.secondsLooped = secondsLooped;
            hitBoxScript.parent = parent;

            // Get the AudioSource component and play the attack sound
            AudioSource audioSource = techGo.GetComponent<AudioSource>();
            if (audioSource != null && attackSound != null)
            {
                audioSource.clip = attackSound;
                audioSource.loop = true; // Set the audio to loop
                audioSource.Play();
            }

            animationScript.GetFrames();
            animationScript.StartAnimation();

            // Start a coroutine to stop the audio after the animation duration
            StartCoroutine(StopAudioAfterDuration(audioSource, secondsLooped));
        }

        private IEnumerator StopAudioAfterDuration(AudioSource audioSource, float duration)
        {
            yield return new WaitForSeconds(duration);
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
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
        
        public override TechEnum GetTechEnum()
        {
            return TechEnum.FireRain;
        }
    }
}
