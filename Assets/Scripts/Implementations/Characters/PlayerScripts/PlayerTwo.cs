using BaseClasses;
using UnityEngine;

namespace Implementations.Characters.PlayerScripts
{
    public class PlayerTwo : Player
    {
        protected override void AwakeWrapper()
        {
            base.AwakeWrapper();
        
            UpCode = KeyCode.UpArrow;
            DownCode = KeyCode.DownArrow;
            LeftCode = KeyCode.LeftArrow;
            RightCode = KeyCode.RightArrow;

            AttackCode = KeyCode.Comma;
            FirstTechnique = KeyCode.N;
            SecondTechnique = KeyCode.M;
        }
    }
}