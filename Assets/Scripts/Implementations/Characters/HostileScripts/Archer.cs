using BaseClasses;
using UnityEngine;

namespace Implementations.Characters.HostileScripts
{
    public class Archer : TrackingBehavior
    {
        protected override void StartWrapper()
        {
            Hp = 10;
            Mana = 0;
            base.StartWrapper();
        }

        public override void Defeated()
        {
            throw new System.NotImplementedException();
        }
    }
}