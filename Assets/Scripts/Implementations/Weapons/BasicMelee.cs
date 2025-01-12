using System.Collections;
using System.Collections.Generic;
using BaseClasses;
using UnityEngine;

namespace Implementations.Weapons
{
    public class BasicMelee : Weapon
    {
        public int damage;

        protected override void StartWrapper()
        {
            base.StartWrapper();
            AnimationDuration = 0;
            Ignore = new List<CharacterSheet>(new []{ParentCs});
        }
        protected override void Effect(CharacterSheet cs)
        {
            cs.DealDamage(damage);
        }
        protected override IEnumerator PlayAnimation()
        {
            throw new System.NotImplementedException();
        }
    }
}