using BaseClasses;
using UnityEngine;

namespace Implementations.Characters.HostileScripts
{
    public class Archer : TrackingBehavior
    {
        protected override void StartWrapper()
        {
            maxHp = 10;
            base.StartWrapper();
        }

        public override void Defeated()
        {
            throw new System.NotImplementedException();
        }
    }
}