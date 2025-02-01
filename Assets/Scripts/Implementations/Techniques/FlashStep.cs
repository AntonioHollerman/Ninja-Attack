using System;
using System.Collections;
using BaseClasses;
using Implementations.Extras;
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

        private float _dashAnimationDuration;
        protected override void StartWrapper()
        {
            base.StartWrapper();
            MultiSpriteAnimation dashAni = Instantiate(dashPrefab).GetComponent<MultiSpriteAnimation>();
            LoopAnimation redSparkAni = Instantiate(redSparkPrefab).GetComponent<LoopAnimation>();

            _dashAnimationDuration = dashAni.GetDuration();
            animationBlockDuration = dashAni.GetDuration() + redSparkAni.GetDuration();
            
            Destroy(dashAni.gameObject);
            Destroy(redSparkAni.gameObject);
        }

        public override void Execute()
        {
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
            GameObject redSpark = Instantiate(redSparkPrefab, cs.transform.position, cs.transform.rotation);
            redSpark.transform.Translate(sparkOffset);
            if (cs is Player playerScript)
            {
                NormalizeSpriteDirection(redSpark.transform, playerScript.transform.forward);
            }
            redSpark.GetComponent<LoopAnimation>().StartAnimation();
        }

        private MultiSpriteAnimation DashAnimation()
        {
            GameObject dash = Instantiate(dashPrefab, cs.transform.position, cs.transform.rotation);
            if (cs is Player playerScript)
            {
                NormalizeSpriteDirection(dash.transform, playerScript.transform.forward);
            }
            
            return dash.GetComponent<MultiSpriteAnimation>();
        }
        
        private void BlueSparkAnimation()
        {
            Vector3 dirInverse = cs.transform.forward * -1;
            GameObject blueSpark = Instantiate(blueSparkPrefab, cs.transform.position, blueSparkPrefab.transform.rotation);
            blueSpark.transform.localRotation = Quaternion.LookRotation(dirInverse);
            blueSpark.transform.Translate(sparkOffset);
            if (cs is Player)
            {
                NormalizeSpriteDirection(blueSpark.transform, dirInverse);
            }
            blueSpark.GetComponent<LoopAnimation>().StartAnimation();
        }

        private IEnumerator PlayAnimation()
        {
            if (cs is Player player)
            {
                player.BlockInput(animationBlockDuration);
            }
            
            RedSparkAnimation();
            MultiSpriteAnimation dashScript = DashAnimation();
            StartCoroutine(MoveParent(dashScript));
            dashScript.StartAnimation(3);
            yield return new WaitUntil(() => dashScript == null);
            BlueSparkAnimation();
        }

        private IEnumerator MoveParent(MultiSpriteAnimation dashScript)
        {
            Vector3 startPos = cs.transform.position;
            Vector3 maxDistance = startPos + cs.transform.forward * distance;
            cs.rb.velocity = cs.transform.forward * 20;
            yield return new WaitUntil(() => Vector3.Distance(startPos, cs.transform.position) > distance || dashScript == null);
            if (Vector3.Distance(startPos, cs.transform.position) > distance)
            {
                cs.transform.position = maxDistance;
            }
            cs.rb.velocity = Vector3.zero;
        }
    }
}