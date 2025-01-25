using BaseClasses;
using UnityEngine;

namespace Implementations.Characters.PlayerScripts
{
    public class PlayerOne : Player
    {
        protected override void AwakeWrapper()
        {
            base.AwakeWrapper();
        
            UpCode = KeyCode.W;
            DownCode = KeyCode.S;
            LeftCode = KeyCode.A;
            RightCode = KeyCode.D;

            AttackCode = KeyCode.Space;
            FirstTechnique = KeyCode.Q;
            SecondTechnique = KeyCode.E;
        }

        public override void Defeated()
        {
            throw new System.NotImplementedException();
        }
    }
}