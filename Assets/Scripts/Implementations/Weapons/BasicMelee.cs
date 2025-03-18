using System.Collections;
using Implementations.Extras;
using BaseClasses;
using UnityEngine;

namespace Implementations.Weapons
{
    public class BasicMelee : Weapon
    {
        public AudioClip SwordSwing; // Sound effect for the melee attack
        public AudioClip HitSound; // Sound effect for when the melee hits a target
        public AudioClip SwordClash;
        private AudioSource audioSource; // Reference to the AudioSource component

        protected override void AwakeWrapper()
        {
            base.AwakeWrapper();
            audioSource = GetComponent<AudioSource>(); // Get the AudioSource component
            AnimationDuration = frames.Count * secondsBetweenFrame; // Set animation duration to the attack delay
            Attack();
        }

        protected override void Effect(CharacterSheet cs)
        {
   

            Vector3 pos = cs.gameObject.transform.position;
            GameObject prefab = Resources.Load<GameObject>(CharacterSheet.HitMarkPath);
            LoopAnimation script = Instantiate(prefab, pos, prefab.transform.rotation).GetComponent<LoopAnimation>();

            script.StartAnimation();
            cs.DealDamage(0.85f * parent.Atk, parent); // Apply damage to the target

            PlayHitSound();// Play the attack sound
        }

        protected override IEnumerator Execute()
        {
            PlayAttackSound();
            yield return null;
        }

        protected override void TriggerEnterWrapper(Collider other)
        {
            base.TriggerEnterWrapper(other);
            BasicArrow arr = other.gameObject.GetComponent<BasicArrow>();
            if (arr != null)
            {
                Vector3 pos = arr.gameObject.transform.position;
                GameObject prefab = Resources.Load<GameObject>(CharacterSheet.HitMarkPath);
                PlayClashSound();
                LoopAnimation script = Instantiate(prefab, pos, prefab.transform.rotation).GetComponent<LoopAnimation>();

                script.StartAnimation();
                Destroy(other.gameObject);
            }
        }

        private void PlayAttackSound()
        {
            audioSource.PlayOneShot(SwordSwing); // Play the attack sound
        }

        private void PlayClashSound()
        {
            audioSource.PlayOneShot(SwordClash); // Play the attack sound
        }

        private void PlayHitSound(float volume = 0.5f)
        {
            audioSource.PlayOneShot(HitSound, volume); // Play the hit sound with specified volume
        }
    }
}