using System.Collections;
using BaseClasses;
using Implementations.Animations;
using UnityEngine;

namespace Implementations.Weapons
{
    public class Arrow : HitBox
    {
        public float atkMultiplier;
        public float speed;
        protected override void Effect(CharacterSheet cs)
        {
            cs.DealDamage(atkMultiplier * parent.Atk, parent);
            SpawnHitMark(cs.pTransform.gameObject);
            Destroy(gameObject);
        }

        protected override void TriggerEnterWrapper(Collider other)
        {
            CharacterSheet cs = other.GetComponent<CharacterSheet>();
            if (cs == null && !other.CompareTag("CharacterSheet"))
            {
                SpawnHitMark(gameObject);
                Destroy(gameObject);
            }
        }

        protected override void AwakeWrapper()
        {
            Activate(10);
        }

        protected override void UpdateWrapper()
        {
            transform.Translate(Time.deltaTime * speed * Vector3.forward);
        }
        
        private void SpawnHitMark(GameObject target)
        {
            GameObject hitMarkPrefab = Resources.Load<GameObject>(CharacterSheet.HitMarkPath);
            GameObject hitMarkGo = Instantiate(
                hitMarkPrefab, 
                target.transform.position, 
                hitMarkPrefab.transform.rotation
            );
            LoopAnimation ani = hitMarkGo.GetComponent<LoopAnimation>();
            ani.StartAnimation();
        }
    }
}