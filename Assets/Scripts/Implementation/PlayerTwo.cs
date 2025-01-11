using BaseClasses;
using UnityEngine;

namespace Implementation
{
    public class PlayerTwo : Player
    {
        protected override void StartWrapper()
        {
            base.StartWrapper();
            speed = 5;
        
            UpCode = KeyCode.UpArrow;
            DownCode = KeyCode.DownArrow;
            LeftCode = KeyCode.LeftArrow;
            RightCode = KeyCode.RightArrow;
        }
    }
}