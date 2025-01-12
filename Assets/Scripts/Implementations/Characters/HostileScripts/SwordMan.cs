using System.Collections;
using System.Collections.Generic;
using BaseClasses;
using Implementations.Weapons;
using UnityEngine;

namespace Implementations.Characters.HostileScripts
{
    public class SwordMan : TrackingBehavior
    {
        public GameObject meleeGo;
        public float speed = 2f;
        public float attackDelay;
        public float attackRange;
        
        private float _attackTimer;
        private bool InAttackRange => TargetDistance <= attackRange;
        private bool AttackReady => _attackTimer <= 0;
        private BasicMelee _meleeScript;
        protected override void StartWrapper()
        {
            base.StartWrapper();
            _meleeScript = meleeGo.GetComponent<BasicMelee>();
        }

        protected override void UpdateWrapper()
        {
            base.UpdateWrapper();
            FollowTarget();
        }
        
        
        private IEnumerator AttackTarget()
        {
            while (true)
            {
                yield return new WaitUntil(() => InAttackRange && AttackReady);
                _meleeScript.Execute();
                _attackTimer = attackDelay;
            }
        }
        
        private void FollowTarget()
        {
            Vector3 direction = (Target.transform.position - transform.position).normalized;
            transform.position +=  speed * Time.deltaTime * direction;
        }

        public override void Defeated()
        {
            throw new System.NotImplementedException();
        }
    }
}