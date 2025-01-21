using BaseClasses;
using UnityEngine;

namespace Implementations.Characters.PlayerScripts
{
    public class PlayerTwo : Player
    {
        protected override void StartWrapper()
        {
            speed = 5;
            MaxHp = 20;
            MaxMana = 150;
            base.StartWrapper();
        
            UpCode = KeyCode.UpArrow;
            DownCode = KeyCode.DownArrow;
            LeftCode = KeyCode.LeftArrow;
            RightCode = KeyCode.RightArrow;
        }

        public override void Defeated()
        {
            throw new System.NotImplementedException();
        }
    }
}