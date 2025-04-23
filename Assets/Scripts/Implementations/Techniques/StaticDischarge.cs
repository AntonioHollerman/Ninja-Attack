using System.Collections;
using BaseClasses;
using Implementations.Animations;
using Implementations.Animations.UniqueAnimation;
using Implementations.Extras;
using Implementations.HitBoxes;
using UnityEngine;

namespace Implementations.Techniques
{
    public class StaticDischarge : Technique
    {
        public GameObject dischargeAnimationPrefab;
        public int frameStartHitBox;

        public AudioClip dischargeSound; // Sound effect for the static discharge
        private AudioSource audioSource; // Reference to the AudioSource component

        protected override void Execute()
        {
            GameObject techGo = Instantiate(dischargeAnimationPrefab);
            StaticDischargeAnimation animationScript = techGo.GetComponent<StaticDischargeAnimation>();
            ElectricHitBox hitBoxScript = techGo.GetComponent<ElectricHitBox>();
            hitBoxScript.parent = parent;
            hitBoxScript.parentTech = this;

            StartCoroutine(TrackParent(animationScript));
            StartCoroutine(FramesListener(animationScript, hitBoxScript));
            animationScript.StartAnimation();

            PlayDischargeSound(); // Play the discharge sound
        }

        private IEnumerator FramesListener(LoopAnimation ani, ElectricHitBox hitBox)
        {
            yield return new WaitUntil(() => ani.FrameIndex >= frameStartHitBox);
            int framesLeft = ani.frames.Length - ani.FrameIndex;
            hitBox.Activate(framesLeft * ani.SecondsBetweenFrame);
        }

        private IEnumerator TrackParent(LoopAnimation ani)
        {
            while (true)
            {
                if (ani == null)
                {
                    break;
                }
                ani.transform.position = parent.transform.position;
                yield return null;
            }
        }

        protected override void StartWrapper()
        {
            base.StartWrapper();
            audioSource = GetComponent<AudioSource>();
            LoopAnimation animationScript = Instantiate(dischargeAnimationPrefab).GetComponent<LoopAnimation>();
            Destroy(animationScript.gameObject);
        }

        private void PlayDischargeSound()
        {
            if (audioSource != null && dischargeSound != null)
            {
                audioSource.PlayOneShot(dischargeSound); // Play the discharge sound
            }
        }

        protected override float GetSpellCastDuration()
        {
            return dischargeAnimationPrefab.GetComponent<LoopAnimation>().GetAnimationDuration();
        }
    }
}