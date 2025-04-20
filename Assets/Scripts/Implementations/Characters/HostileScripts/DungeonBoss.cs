using System.Collections;
using BaseClasses;
using Implementations.Extras;
using Implementations.Managers;
using UnityEngine;

namespace Implementations.Characters.HostileScripts
{
    public class DungeonBoss : Hostile
    {
        private Technique _tech1;
        private Technique _tech2;
        private Technique _tech3;

        protected override void AwakeWrapper()
        {
            base.AwakeWrapper();
            AddTarget(GameObject.Find("SoloPlayer"));

            StartCoroutine(WaitTillTechManagerActive());
        }
        
        private IEnumerator WaitTillTechManagerActive()
        {
            yield return new WaitUntil(() => TechniqueManager.Instance != null);
            _tech1 = TechniqueManager.Instance.LoadTechnique(TechEnum.ElectricWhip, this);
            _tech2 = TechniqueManager.Instance.LoadTechnique(TechEnum.FireSummmon, this);
            _tech3 = TechniqueManager.Instance.LoadTechnique(TechEnum.FireRain, this);
            
            
            
            StartCoroutine(MeleeListener());
            StartCoroutine(TechOneListener());
            StartCoroutine(TechTwoListener());
            StartCoroutine(TechThreeListener());
        } 

        private IEnumerator MeleeListener()
        {
            yield return null;
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
            while (true)
            {
                yield return new WaitForSeconds(5);
                _tech2.ActivateTech();
            }
        }
        
        private IEnumerator TechThreeListener()
        {
            while (true)
            {
                yield return new WaitForSeconds(7);
                _tech3.ActivateTech();
            }
        }
    }
}