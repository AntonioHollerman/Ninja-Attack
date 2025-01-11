using BaseClasses;
using UnityEngine;

namespace Implementation
{
    public class PlayerOne : Player
    {
        protected override void StartWrapper()
        {
            base.StartWrapper();
            speed = 5;
        
            UpCode = KeyCode.W;
            DownCode = KeyCode.S;
            LeftCode = KeyCode.A;
            RightCode = KeyCode.D;
        }
    }
}