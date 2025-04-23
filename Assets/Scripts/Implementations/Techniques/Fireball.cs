using System.Collections;
using BaseClasses;
using Implementations.Animations;
using Implementations.Extras;
using Implementations.HitBoxes;
using UnityEngine;
using AnimationState = Implementations.Animations.CharacterAnimation.AnimationState;

namespace Implementations.Techniques
{
    public class Fireball : Technique
    {
        private enum State
        {
            FIRST_ACTIVATION_NOT_READY, FIRST_ACTIVATION_READY, SECOND_ACTIVATION_READY
        }
        
        public GameObject explosionPrefab;
        public GameObject fireballPrefab;
        public float flySpeed;
        public float zDisplacement;

        public AudioClip fireballSound; // Sound effect for the fireball flying
        public AudioClip explosionSound; // Sound effect for the explosion
        private AudioSource audioSource; // Reference to the AudioSource component

        private State _curState = State.FIRST_ACTIVATION_NOT_READY;
        private GameObject _curFireball;
        private bool _activationBeingProcessed;
        
        protected override void Execute()
        {
            throw new System.NotImplementedException();
        }

        public override void ActivateTech()
        {
            if (_activationBeingProcessed)
            {
                return;
            }
            if (_curState is State.FIRST_ACTIVATION_READY)
            {
                _activationBeingProcessed = true;
                FirstActivation();
                _activationBeingProcessed = false;
            }
            else if (_curState is State.SECOND_ACTIVATION_READY)
            {
                _activationBeingProcessed = true;
                SecondActivation();
                _activationBeingProcessed = false;
            }
        }

        protected override IEnumerator StateListener()
        {
            _curState = Ready ? State.FIRST_ACTIVATION_READY : State.FIRST_ACTIVATION_NOT_READY;
            if (_curState is State.FIRST_ACTIVATION_NOT_READY)
            {
                countDown.gameObject.SetActive(true);
                boarder.gameObject.SetActive(false);
                icon.sprite = notActive;
            }

            while (true)
            {
                yield return new WaitUntil(() => _curState is State.FIRST_ACTIVATION_READY);
                countDown.gameObject.SetActive(false);
                boarder.gameObject.SetActive(false);
                icon.sprite = active;
                yield return new WaitUntil(() => _curState is State.SECOND_ACTIVATION_READY);
                countDown.gameObject.SetActive(false);
                boarder.gameObject.SetActive(true);
                icon.sprite = active;
                yield return new WaitUntil(() => _curState is State.FIRST_ACTIVATION_NOT_READY or State.FIRST_ACTIVATION_READY);
                countDown.gameObject.SetActive(true);
                boarder.gameObject.SetActive(false);
                icon.sprite = notActive;
                yield return new WaitUntil(() => Ready);
                _curState = State.FIRST_ACTIVATION_READY;
            }
        }

        private bool ReadyToCast()
        {
            if (parent == null)
            {
                Destroy(gameObject);
            }
            if (!Ready)
            {
                return false;
            }

            bool successful = parent.CastTechnique(ManaCost);
            return successful;
        }

        private void NormalizeSpriteDirection(Transform sprite, Player playerScript)
        {
            if (playerScript.transform.forward.x != 0)
            {
                sprite.transform.localRotation = Quaternion.Euler(0, 90, -90);
            }
        }

        private void FirstActivation()
        {
            audioSource = GetComponent<AudioSource>();
            if (!ReadyToCast())
            {
                return;
            }

            parent.body.curState = AnimationState.SpellCast;
            parent.BlockAnimation(animationBlockDuration);
            
            _curFireball = Instantiate(fireballPrefab,
                parent.transform.position,
                parent.transform.rotation);
            _curFireball.transform.Translate(Vector3.forward * zDisplacement);

            if (parent is Player playerScript)
            {
                Transform sprite = _curFireball.transform.Find("sprite");
                NormalizeSpriteDirection(sprite, playerScript);
            }

            FireballCollider ballCollider = _curFireball.GetComponent<FireballCollider>();
            ballCollider.parentTech = this;
            ballCollider.speed = _curFireball.transform.forward * flySpeed;

            _curFireball.GetComponent<LoopAnimation>().StartAnimation();

            Rigidbody ballRb = _curFireball.GetComponent<Rigidbody>();
            ballRb.velocity = _curFireball.transform.forward * flySpeed;

            PlayFireballSound(); // Play the fireball flying sound

            _curState = State.SECOND_ACTIVATION_READY;
        }

        private void SecondActivation()
        {
            Vector3 pos = _curFireball.transform.position;

            Destroy(_curFireball.gameObject);
            GameObject explosion = Instantiate(explosionPrefab, pos, explosionPrefab.transform.rotation);

            LoopAnimation animationScript = explosion.GetComponent<LoopAnimation>();
            FireHitBox hitBox = explosion.GetComponent<FireHitBox>();

            hitBox.parent = parent;
            hitBox.Activate(animationScript.frames.Length * animationScript.SecondsBetweenFrame);
            animationScript.StartAnimation();

            PlayExplosionSound(); // Play the explosion sound

            Timer = CoolDown;
            _curState = Ready ? State.FIRST_ACTIVATION_READY : State.FIRST_ACTIVATION_NOT_READY;
        }

        private void PlayFireballSound()
        {
            if (audioSource != null && fireballSound != null)
            {
                audioSource.PlayOneShot(fireballSound); // Play the fireball sound
            }
        }

        private void PlayExplosionSound()
        {
            if (audioSource != null && explosionSound != null)
            {
                audioSource.PlayOneShot(explosionSound); // Play the explosion sound
            }
        }
    }
}