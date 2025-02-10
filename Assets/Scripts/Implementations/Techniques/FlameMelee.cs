using BaseClasses;
using Implementations.Extras;
using Implementations.HitBoxes;
using System;
using System.Collections;
using UnityEngine;

namespace Implementations.Techniques
{
    public class FlameMelee : Technique
    {
        public GameObject meleeAnimationPrefab;
        public float zOffset;
        public int frameStartHitBox;

        public AudioClip fireSwordSound; // Sound effect for the fire sword
        public AudioClip hitSound; // Sound effect for when the melee hits a target
        private AudioSource audioSource; // Reference to the AudioSource component

        public override void Execute()
        {
            GameObject techGo = Instantiate(meleeAnimationPrefab);
            LoopAnimation animationScript = techGo.GetComponent<LoopAnimation>();
            FireHitBox hitBoxScript = techGo.GetComponent<FireHitBox>();
            hitBoxScript.parent = parent;

            StartCoroutine(TrackParent(animationScript));
            StartCoroutine(FramesListener(animationScript, hitBoxScript));
            animationScript.StartAnimation();

            PlayFireSwordSound(); // Play the fire sword sound
            PlayHitSound();
        }

        private IEnumerator FramesListener(LoopAnimation ani, FireHitBox hitBox)
        {
            yield return new WaitUntil(() => ani.FrameIndex >= frameStartHitBox);
            int framesLeft = ani.frames.Length - ani.FrameIndex;
            hitBox.Activate(framesLeft * ani.SecondsBetweenFrame);
        }

        private void NormalizeSpriteDirection(Transform sprite, Player playerScript)
        {
            if (playerScript.transform.forward == Vector3.right || playerScript.transform.forward == Vector3.left)
            {
                try
                {
                    sprite.localRotation = Quaternion.Euler(0, 90, 90);
                }
                catch (Exception ignore)
                {
                    // ignored
                }
                return;
            }
            if (playerScript.transform.forward == Vector3.up || playerScript.transform.forward == Vector3.down)
            {
                try
                {
                    sprite.localRotation = Quaternion.Euler(270, 90, 90);
                }
                catch (Exception ignore)
                {
                    // ignored
                }
            }
        }

        private IEnumerator TrackParent(LoopAnimation ani)
        {
            if (parent is Player playerScript)
            {
                Transform sprite = ani.transform.Find("sprite");
                NormalizeSpriteDirection(sprite, playerScript);
            }
            ani.transform.rotation = parent.transform.rotation;
            while (true)
            {
                if (ani == null)
                {
                    break;
                }

                ani.transform.position = parent.transform.position;
                ani.transform.Translate(Vector3.forward * zOffset);

                yield return null;
            }
        }

        protected override void StartWrapper()
        {
            base.StartWrapper();
            audioSource = GetComponent<AudioSource>();
            LoopAnimation animationScript = Instantiate(meleeAnimationPrefab).GetComponent<LoopAnimation>();
            animationBlockDuration = animationScript.GetAnimationDuration();
            Destroy(animationScript.gameObject);
        }

        private void PlayFireSwordSound()
        {
            if (audioSource != null && fireSwordSound != null)
            {
                audioSource.PlayOneShot(fireSwordSound); // Play the fire sword sound
            }
        }

        private void PlayHitSound(float volume = 0.5f)
        {
            if (audioSource != null && hitSound != null)
            {
                audioSource.PlayOneShot(hitSound); // Play the hit sound
            }

    }
    }
}