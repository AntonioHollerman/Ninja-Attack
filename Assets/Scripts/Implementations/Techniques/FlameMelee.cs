using System.Collections;
using BaseClasses;
using Implementations.Extras;
using Implementations.Techniques.HitBoxes;
using UnityEngine;
using UnityEngine.Serialization;

namespace Implementations.Techniques
{
    public class FlameMelee : Technique
    {
        public GameObject meleeAnimationPrefab;
        public float xOffset;
        public override void Execute()
        { 
            LoopAnimation animationScript = Instantiate(meleeAnimationPrefab).GetComponent<LoopAnimation>();
            animationScript.StartAnimation();
            StartCoroutine(TrackParent(animationScript));
        }

        private IEnumerator TrackParent(LoopAnimation ani)
        {
            Player playerScript = cs.GetComponent<Player>();
            bool notPlayer = playerScript != null;
            Transform sprite = ani.transform.Find("sprite");
            while (true)
            {
                if (ani == null)
                {
                    break;
                }
                
                ani.transform.rotation = cs.transform.rotation;
                ani.transform.position = cs.transform.position;
                ani.transform.Translate(Vector3.forward * xOffset);

                if (notPlayer)
                {
                    yield return null;
                }
                
                if (cs.transform.forward == Vector3.right || cs.transform.forward == Vector3.left)
                {
                    sprite.localRotation = Quaternion.Euler(0, 90, 90);
                    yield return null;
                }
                if (Input.GetKey(playerScript.leftCode) || Input.GetKey(playerScript.rightCode))
                {
                    sprite.localRotation = Quaternion.Euler(0, 90, 90);
                    yield return null;
                }
                if (cs.transform.forward == Vector3.up || cs.transform.forward == Vector3.down)
                {
                    sprite.localRotation = Quaternion.Euler(270, 90, 90);
                }
                yield return null;
            }
        }

        protected override void StartWrapper()
        {
            base.StartWrapper();
            LoopAnimation animationScript = Instantiate(meleeAnimationPrefab).GetComponent<LoopAnimation>();
            animationBlockDuration = animationScript.GetAnimationDuration();
            Destroy(animationScript.gameObject);
        }
    }
}