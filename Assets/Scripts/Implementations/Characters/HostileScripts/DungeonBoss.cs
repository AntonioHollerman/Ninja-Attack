using System.Collections;
using BaseClasses;
using Implementations.Managers;
using UnityEngine;

namespace Implementations.Characters.HostileScripts
{
    public class DungeonBoss : TrackingBehavior
    {
        private Technique _tech1;
        private Technique _tech2;
        private Technique _tech3;

        protected override void AwakeWrapper()
        {
            base.AwakeWrapper();

            _tech1 = TechniqueManager.Instance.LoadTechnique(TechEnum.ElectricWhip, this);

            StartCoroutine(MeleeListener());
            StartCoroutine(TechOneListener());
            StartCoroutine(TechTwoListener());
            StartCoroutine(TechThreeListener());
        }

        private IEnumerator MeleeListener()
        {
            return null;
        }

        private IEnumerator TechOneListener()
        {
            while (true)
            {
                yield return new WaitForSeconds(3);
                _tech1.ActivateTech();
            }
        }
        
        private IEnumerator TechTwoListener()
        {
            return null;
        }
        
        private IEnumerator TechThreeListener()
        {
            return null;
        }
    }
}