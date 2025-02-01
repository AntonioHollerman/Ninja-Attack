using System.Collections;
using Implementations.Extras;
using BaseClasses;
using UnityEngine;

namespace Implementations.Weapons
{
    public class BasicMelee : Weapon
    {
        public int damage; // Damage dealt by the melee attack
        public AudioClip SwordSwing; // Sound effect for the melee attack
        public AudioClip HitSound; // Sound effect for when the melee hits a target
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
            cs.DealDamage(damage); // Apply damage to the target

            PlayHitSound(); // Play the hit sound
            PlayAttackSound(); // Play the attack sound
        }

        protected override IEnumerator Execute()
        {
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
                LoopAnimation script = Instantiate(prefab, pos, prefab.transform.rotation).GetComponent<LoopAnimation>();

                script.StartAnimation();
                Destroy(other.gameObject);
            }
        }

        private void PlayAttackSound()
        {
            if (audioSource != null && SwordSwing != null)
            {
                audioSource.PlayOneShot(SwordSwing); // Play the attack sound
            }
        }

        private void PlayHitSound()
        {
            if (audioSource != null && HitSound != null)
            {
                audioSource.PlayOneShot(HitSound); // Play the hit sound
            }
        }
    }
}