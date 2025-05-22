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
            parent.StartCoroutine(SpawnEffect(cs.transform.parent));
            cs.Vulnerable(effectDuration);
            
        }

        private IEnumerator SpawnEffect(Transform target)
        {
            GameObject effect = Instantiate(iceHitMark, target.position, iceHitMark.transform.rotation);
            float time = 0;
            while (time < effectDuration)
            {
                effect.transform.position = target.transform.position;
                time += Time.deltaTime;
                yield return null;
            }
            Destroy(effect);
        }
    }
}