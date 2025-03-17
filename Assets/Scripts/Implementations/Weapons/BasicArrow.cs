using System.Collections;
using BaseClasses;
using Implementations.Extras;
using UnityEngine;

namespace Implementations.Weapons
{
    public class BasicArrow : Weapon
    {
        public int damage;
        public float speed = 5f; // Speed of the arrow
        public float maxTravelDistance = 15f; // Maximum distance the arrow can travel before deactivating
        public AudioClip arrowSound; // Sound effect for the arrow
        public AudioClip hitSound; // Sound effect for when the arrow hits a target
        private AudioSource audioSource; // Reference to the AudioSource component

        protected override void AwakeWrapper()
        {
            base.AwakeWrapper();
            audioSource = GetComponent<AudioSource>(); // Get the AudioSource component
            AnimationDuration = speed != 0 ? maxTravelDistance / speed : 3; // Calculate duration based on speed and distance
        }

        protected override void Effect(CharacterSheet cs)
        {
            Vector3 pos = cs.gameObject.transform.position;
            GameObject prefab = Resources.Load<GameObject>(CharacterSheet.HitMarkPath);
            LoopAnimation script = Instantiate(prefab, pos, prefab.transform.rotation).GetComponent<LoopAnimation>();

            Deactivate();
            script.StartAnimation();
            cs.DealDamage(damage, parent); // Apply damage to the target

            PlayHitSound(); // Play the hit sound
            PlayArrowSound(); // Play the arrow sound
        }

        protected override IEnumerator Execute()
        {
            float traveledDistance = 0f; // Track how far the arrow has traveled

            PlayArrowSound(); // Play the arrow sound when the arrow is shot

            while (traveledDistance < maxTravelDistance)
            {
                transform.Translate(speed * Time.deltaTime * Vector3.forward);
                traveledDistance += speed * Time.deltaTime; // Update traveled distance
                yield return null;
            }

            Deactivate(); // Deactivate the arrow after traveling the maximum distance
        }

        private void PlayArrowSound()
        {
            if (audioSource != null && arrowSound != null)
            {
                audioSource.PlayOneShot(arrowSound); // Play the arrow sound
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