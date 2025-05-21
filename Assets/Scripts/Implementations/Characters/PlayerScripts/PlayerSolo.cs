using System.Collections;
using BaseClasses;
using Implementations.Managers;
using UnityEngine;

namespace Implementations.Characters.PlayerScripts
{
    public class PlayerSolo : Player
    {
        protected override void AwakeWrapper()
        {
            base.AwakeWrapper();
        
            upCode = KeyCode.W;
            downCode = KeyCode.S;
            leftCode = KeyCode.A;
            rightCode = KeyCode.D;
            interactCode = KeyCode.F;

            attackCode = KeyCode.Space;
            firstTechniqueCode = KeyCode.Q;
            secondTechniqueCode = KeyCode.E;
            
            StartCoroutine(WaitTillTechManagerActive());
        }

        private IEnumerator WaitTillTechManagerActive()
        {
            yield return new WaitUntil(() => TechniqueManager.Instance != null);
            techOne = TechniqueManager.Instance.LoadPlayerOneTechnique(
                TechEnum.IceShield, 
                this, 
                0);
            // techOne = TechniqueManager.Instance.LoadPlayerOneTechnique(
            //     TechEnum.FlashStep, 
            //     this, 
            //     0);
            // techTwo = TechniqueManager.Instance.LoadPlayerOneTechnique(
            //     TechEnum.FireSword,
            //     this, 
            //     1);
        } 
    }
}