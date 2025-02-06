using System.Collections;
using BaseClasses;
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

        protected override void StartWrapper()
        {
            base.StartWrapper();
            _parentCollider = parent.gameObject.GetComponent<Collider>();

            MultiSpriteAnimation dashAni = Instantiate(dashPrefab).GetComponent<MultiSpriteAnimation>();
            LoopAnimation redSparkAni = Instantiate(redSparkPrefab).GetComponent<LoopAnimation>();

            animationBlockDuration = dashAni.GetDuration() + redSparkAni.GetDuration();
            _dashDuration = dashAni.GetDuration();

            Destroy(dashAni.gameObject);
            Destroy(redSparkAni.gameObject);
        }

        public override void Execute()
        {
            foreach (CharacterSheet cs in CharacterSheet.CharacterSheets)
            {
                if (cs == parent)
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
            GameObject redSpark = Instantiate(redSparkPrefab, parent.transform.position, parent.transform.rotation);
            redSpark.transform.Translate(sparkOffset);
            if (parent is Player playerScript)
            {
                NormalizeSpriteDirection(redSpark.transform, playerScript.transform.forward);
            }
            redSpark.GetComponent<LoopAnimation>().StartAnimation();
        }

        private MultiSpriteAnimation DashAnimation()
        {
            GameObject dash = Instantiate(dashPrefab, parent.transform.position, parent.transform.rotation);
            if (parent is Player playerScript)
            {
                NormalizeSpriteDirection(dash.transform, playerScript.transform.forward);
            }

            return dash.GetComponent<MultiSpriteAnimation>();
        }

        private void BlueSparkAnimation()
        {
            Vector3 dirInverse = parent.transform.forward * -1;
            GameObject blueSpark = Instantiate(blueSparkPrefab, parent.transform.position, blueSparkPrefab.transform.rotation);
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
                player.BlockInput(animationBlockDuration);
            }

            RedSparkAnimation();
            MultiSpriteAnimation dashScript = DashAnimation();

            ElectricHitBox hb = dashScript.gameObject.GetComponent<ElectricHitBox>();
            hb.parent = parent;
            hb.parentTech = this;
            hb.Activate(_dashDuration);

            StartCoroutine(MoveParent(dashScript));
            dashScript.StartAnimation(3);
            yield return new WaitUntil(() => dashScript == null);
            BlueSparkAnimation();
        }

        private IEnumerator MoveParent(MultiSpriteAnimation dashScript)
        {
            Vector3 startPos = parent.transform.position;
            Vector3 maxDistance = startPos + parent.transform.forward * distance;
            parent.rb.velocity = parent.transform.forward * 20;
            yield return new WaitUntil(() => Vector3.Distance(startPos, parent.transform.position) > distance || dashScript == null);
            if (Vector3.Distance(startPos, parent.transform.position) > distance)
            {
                parent.transform.position = maxDistance;
            }

            parent.rb.velocity = Vector3.zero;
            foreach (CharacterSheet cs in CharacterSheet.CharacterSheets)
            {
                if (cs == parent)
                    continue;
            }
            //Physics.IgnoreCollision(_parentCollider, cs.gameObject.GetComponent<Collider>(), false);
        }
    }
}
