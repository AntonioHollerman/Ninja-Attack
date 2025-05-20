using BaseClasses;
using Implementations.Managers;
using UnityEngine;

namespace Implementations.Characters.PlayerScripts
{
    public class PlayerTwo : Player
    {
        protected override void AwakeWrapper()
        {
            base.AwakeWrapper();
        
            upCode = KeyCode.UpArrow;
            downCode = KeyCode.DownArrow;
            leftCode = KeyCode.LeftArrow;
            rightCode = KeyCode.RightArrow;

            attackCode = KeyCode.RightShift;
            firstTechniqueCode = KeyCode.Period;
            secondTechniqueCode = KeyCode.Slash;

            techOne = TechniqueManager.Instance.LoadPlayerTwoTechnique(
                TechEnum.FireBall,
                this, 
                firstTechniqueCode,
                0);
            techTwo = TechniqueManager.Instance.LoadPlayerTwoTechnique(
                TechEnum.FireSword, 
                this, 
                secondTechniqueCode,
                1);
        }
    }
}