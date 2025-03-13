using BaseClasses;
using Implementations.Managers;
using UnityEngine;

namespace Implementations.Characters.PlayerScripts
{
    public class PlayerOne : Player
    {
        protected override void AwakeWrapper()
        {
            base.AwakeWrapper();
        
            upCode = KeyCode.W;
            downCode = KeyCode.S;
            leftCode = KeyCode.A;
            rightCode = KeyCode.D;

            AttackCode = KeyCode.Space;
            FirstTechnique = KeyCode.Q;
            SecondTechnique = KeyCode.E;

            techOne = TechniqueManager.Instance.LoadPlayerOneTechnique(
                TechEnum.ElectricDash, 
                this, 
                FirstTechnique,
           0);
            techTwo = TechniqueManager.Instance.LoadPlayerOneTechnique(
                TechEnum.StaticDischarge,
                this, 
                SecondTechnique, 
                1);
        }
    }
}