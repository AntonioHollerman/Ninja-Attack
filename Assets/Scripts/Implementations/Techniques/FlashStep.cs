﻿using System.Collections;
using BaseClasses;
using Implementations.Animations;
using Implementations.Extras;
using Implementations.HitBoxes;
using UnityEngine;

namespace Implementations.Techniques
{
    public class FlashStep : Technique
    {
        public GameObject blueSparkPrefab;
        public GameObject redSparkPrefab;
        public GameObject dashPrefab;

        public Vector3 sparkOffset;
        public float distance;

        public AudioClip dashSound; // Sound effect for the dash
        private AudioSource audioSource; // Reference to the AudioSource component

        private Collider _parentCollider;
        private float _dashDuration;
        private float _blockInputDuration;

        protected override void StartWrapper()
        {
            base.StartWrapper();
            audioSource = GetComponent<AudioSource>();
            _parentCollider = parent.gameObject.GetComponent<Collider>();

            MultiSpriteAnimation dashAni = Instantiate(dashPrefab).GetComponent<MultiSpriteAnimation>();
            LoopAnimation redSparkAni = Instantiate(redSparkPrefab).GetComponent<LoopAnimation>();

            _blockInputDuration = dashAni.GetDuration();
            _dashDuration = dashAni.GetDuration();

            Destroy(dashAni.gameObject);
            Destroy(redSparkAni.gameObject);
        }

        protected override void Execute()
        {
            foreach (CharacterSheet cs in CharacterSheet.CharacterSheets)
            {
                if (cs == parent || cs == null)
                {
                    continue;
                }
                Physics.IgnoreCollision(_parentCollider, cs.gameObject.GetComponent<Collider>(), true);
            }
            StartCoroutine(PlayAnimation());
        }

        private void NormalizeSpriteDirection(Transform sprite, Vector3 forward)
        {
            if (forward.x != 0)
            {
                if (forward == Vector3.left)
                {
                    sprite.transform.localRotation = Quaternion.Euler(180, 90, -90);
                }
                else if (forward == Vector3.right)
                {
                    sprite.transform.localRotation = Quaternion.Euler(0, 90, -90);
                }
                else
                {
                    sprite.transform.localRotation = Quaternion.Euler(
                        sprite.transform.localEulerAngles.x,
                        sprite.transform.localEulerAngles.y,
                        -90);
                }
            }
        }

        private void RedSparkAnimation()
        {
            GameObject redSpark = Instantiate(redSparkPrefab, parent.pTransform.position, parent.pTransform.rotation);
            redSpark.transform.Translate(sparkOffset);
            if (parent is Player playerScript)
            {
                NormalizeSpriteDirection(redSpark.transform, playerScript.pTransform.forward);
            }
            redSpark.GetComponent<LoopAnimation>().StartAnimation();
        }

        private MultiSpriteAnimation DashAnimation()
        {
            GameObject dash = Instantiate(dashPrefab, parent.pTransform.position, parent.pTransform.rotation);
            if (parent is Player playerScript)
            {
                NormalizeSpriteDirection(dash.transform, playerScript.pTransform.forward);
            }

            return dash.GetComponent<MultiSpriteAnimation>();
        }

        private void BlueSparkAnimation()
        {
            Vector3 dirInverse = parent.pTransform.forward * -1;
            GameObject blueSpark = Instantiate(blueSparkPrefab, parent.pTransform.position, blueSparkPrefab.transform.rotation);
            blueSpark.transform.localRotation = Quaternion.LookRotation(dirInverse);
            blueSpark.transform.Translate(sparkOffset);
            if (parent is Player)
            {
                NormalizeSpriteDirection(blueSpark.transform, dirInverse);
            }
            blueSpark.GetComponent<LoopAnimation>().StartAnimation();
        }

        private IEnumerator PlayAnimation()
        {
            if (parent is Player player)
            {
                player.BlockInput(_blockInputDuration);
            }

            RedSparkAnimation();
            MultiSpriteAnimation dashScript = DashAnimation();

            // Play the dash sound
            PlayDashSound();

            ElectricHitBox hb = dashScript.gameObject.GetComponent<ElectricHitBox>();
            hb.parent = parent;
            hb.parentTech = this;
            hb.Activate(_dashDuration);

            StartCoroutine(MoveParent(dashScript));
            dashScript.StartAnimation(3);
            yield return new WaitUntil(() => dashScript == null);
            BlueSparkAnimation();
        }

        private void PlayDashSound()
        {
            if (audioSource != null && dashSound != null)
            {
                audioSource.PlayOneShot(dashSound); // Play the dash sound
            }
            else
            {
                Debug.LogWarning("AudioSource or dash Sound is not assigned.");
            }
        }

        private IEnumerator MoveParent(MultiSpriteAnimation dashScript)
        {
            Vector3 startPos = parent.pTransform.position;
            Vector3 maxDistance = startPos + parent.pTransform.forward * distance;
            parent.rb.velocity = parent.pTransform.forward * 20;
            yield return new WaitUntil(() => Vector3.Distance(startPos, parent.pTransform.position) > distance || dashScript == null);
            if (Vector3.Distance(startPos, parent.pTransform.position) > distance)
            {
                parent.pTransform.position = maxDistance;
            }

            parent.rb.velocity = Vector3.zero;
            foreach (CharacterSheet cs in CharacterSheet.CharacterSheets)
            {
                if (cs == parent || cs == null)
                {
                    continue;
                }
                
                Physics.IgnoreCollision(_parentCollider, cs.gameObject.GetComponent<Collider>(), false);
            }
        }
        
        public override TechEnum GetTechEnum()
        {
            return TechEnum.FlashStep;
        }
    }
}