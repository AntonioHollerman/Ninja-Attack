using System.Collections;
using BaseClasses;
using Implementations.Weapons;
using UnityEngine;

namespace Implementations.Characters.HostileScripts
{
    public class Archer : Hostile
    {
        public GameObject arrowPrefab;
        public float atkSpeed;
        public float arrowDisplacement;

        protected override void AwakeWrapper()
        {
            base.AwakeWrapper();
            StartCoroutine(StartAttacking());
        }

        private void Attack()
        {
            Vector3 pos = transform.position + arrowDisplacement * transform.forward;
            GameObject arrow = Instantiate(arrowPrefab, 
                pos, 
                transform.rotation);

            BasicArrow script = arrow.GetComponent<BasicArrow>();
            allies = GetAllies();
            script.parent = this;
            script.destroyOnFinish = true;
            script.Attack();
        }

        private IEnumerator StartAttacking()
        {
            while (true)
            {
                yield return new WaitForSeconds(atkSpeed);
                Attack();
            }
        }
    }
}