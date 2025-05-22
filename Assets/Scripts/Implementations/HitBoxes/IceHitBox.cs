using System.Collections;
using BaseClasses;
using UnityEngine;

namespace Implementations.HitBoxes
{
    public class IceHitBox : HitBox
    {
        public GameObject iceHitMark;
        public float effectDuration;
        public float absorbedDmg;
        protected override void Effect(CharacterSheet cs)
        {
            cs.DealDamage(absorbedDmg, parent);
            parent.StartCoroutine(cs.SlowDown(effectDuration));
            parent.StartCoroutine(SpawnEffect(cs));
            cs.Vulnerable(effectDuration);
        }

        private IEnumerator SpawnEffect(CharacterSheet target)
        {
            Transform tarTransform = target.transform.parent;
            GameObject effect = Instantiate(iceHitMark, tarTransform.position, iceHitMark.transform.rotation);
            float time = 0;
            while (time < effectDuration && target.IsALive)
            {
                effect.transform.position = tarTransform.transform.position;
                time += Time.deltaTime;
                yield return null;
            }
            Destroy(effect);
        }
    }
}