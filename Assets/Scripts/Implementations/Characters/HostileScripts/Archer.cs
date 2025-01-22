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

        protected override void StartWrapper()
        {
            base.StartWrapper();
            StartCoroutine(StartAttacking());
        }

        public override void Defeated()
        {
            throw new System.NotImplementedException();
        }

        private void Attack()
        {
            GameObject arrow = Instantiate(arrowPrefab);
            arrow.transform.position = transform.position;
            arrow.transform.rotation = transform.rotation;

            BasicArrow script = arrow.GetComponent<BasicArrow>();
            script.ignore = GetAllies();
            script.destroyOnFinish = true;
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