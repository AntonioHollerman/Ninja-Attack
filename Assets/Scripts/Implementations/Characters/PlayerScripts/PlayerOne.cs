using BaseClasses;
using UnityEngine;

namespace Implementations.Characters.PlayerScripts
{
    public class PlayerOne : Player
    {
        protected override void StartWrapper()
        {
            base.StartWrapper();
        
            UpCode = KeyCode.W;
            DownCode = KeyCode.S;
            LeftCode = KeyCode.A;
            RightCode = KeyCode.D;
        }

        public override void Defeated()
        {
            throw new System.NotImplementedException();
        }
    }
}