using System.Collections;
using BaseClasses;
using Implementations.Weapons;
using UnityEngine;
using AnimationState = Implementations.Animations.CharacterAnimation.AnimationState;

namespace Implementations.Characters.HostileScripts
{
    public class Archer : Hostile
    {
        public float fireRate;
        public int releaseIndex;
        public GameObject arrowPrefab;
        public float forwardDisplacement;

        protected override void AwakeWrapper()
        {
            base.AwakeWrapper();
            AddTarget(GameObject.Find("SoloPlayer"));
            StartCoroutine(AnimationListener());
            StartCoroutine(FireListener());
        }

        private void Shoot()
        {
            Instantiate(arrowPrefab, pTransform.position + pTransform.forward * forwardDisplacement, Quaternion.LookRotation(pTransform.forward, Vector3.forward))
                .GetComponent<Arrow>().parent = this;
        }

        private IEnumerator FireListener()
        {
            while (true)
            {
                while (body.AniIndex < releaseIndex || body.curState != AnimationState.Attack)
                {
                    if (body.curState != AnimationState.Attack && !disable)
                    {
                        rb.velocity = speed * pTransform.forward;
                    }
                    else
                    {
                        rb.velocity = Vector3.zero;
                    }
                    yield return null;
                }
                
                Shoot();
                yield return new WaitUntil(() => body.curState != AnimationState.Attack);
            }
        }

        private IEnumerator AnimationListener()
        {
            while (true)
            {
                yield return new WaitForSeconds(fireRate);
                yield return new WaitUntil(() => !disable && !IsStunned && !UniversalStopCsUpdateLoop);
                body.curState = AnimationState.Attack;
            }
        }
    }
}