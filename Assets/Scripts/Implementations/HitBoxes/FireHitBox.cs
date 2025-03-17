using BaseClasses;
using Implementations.Extras;
using Implementations.Weapons;
using UnityEngine;

namespace Implementations.HitBoxes
{
    public class FireHitBox : HitBox
    {
        public float baseDamage;
        public float burnEffectDps;
        public float duration;
        public bool damageSelf;
        protected override void Effect(CharacterSheet cs)
        {
            cs.DealDamage(baseDamage, parent);
            cs.LoadEffect(((sheet, deltaTime) => sheet.DealDamage(deltaTime * burnEffectDps, parent)), duration);
        }
        
        protected override void TriggerEnterWrapper(Collider other)
        {
            if (damageSelf)
            {
                ActiveIgnore.Remove(parent);
            }
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
    }
}