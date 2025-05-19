using System.Collections;
using BaseClasses;
using Implementations.Extras;
using Implementations.Managers;
using Implementations.Techniques;
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
            disable = true;
            base.AwakeWrapper();
            AddTarget(GameObject.Find("SoloPlayer"));

            StartCoroutine(WaitTillTechManagerActive());
        }
        
        private IEnumerator WaitTillTechManagerActive()
        {
            yield return new WaitUntil(() => TechniqueManager.Instance != null);
            _tech1 = TechniqueManager.Instance.LoadTechnique(TechEnum.ElectricWhip, this);
            _tech2 = TechniqueManager.Instance.LoadTechnique(TechEnum.FireSummon, this);
            _tech3 = TechniqueManager.Instance.LoadTechnique(TechEnum.FireRain, this);
            if (_tech3 is FireRain fireRain)
            {
                fireRain.secondsLooped = 3;
                _tech3 = fireRain;
            }
            
            StartCoroutine(TechOneListener());
            StartCoroutine(TechTwoListener());
            StartCoroutine(TechThreeListener());
        } 

        private IEnumerator TechOneListener()
        {
            while (true)
            {
                yield return new WaitForSeconds(3);
                yield return new WaitUntil(() => !disable && !IsStunned && !UniversalStopCsUpdateLoop);
                _tech1.ActivateTech();
            }
        }
        
        private IEnumerator TechTwoListener()
        {
            while (true)
            {
                yield return new WaitForSeconds(5);
                yield return new WaitUntil(() => !disable && !IsStunned && !UniversalStopCsUpdateLoop);
                _tech2.ActivateTech();
            }
        }
        
        private IEnumerator TechThreeListener()
        {
            while (true)
            {
                yield return new WaitForSeconds(2);
                yield return new WaitUntil(() => !disable && !IsStunned && !UniversalStopCsUpdateLoop);
                _tech3.ActivateTech();
                yield return new WaitForSeconds(3);
            }
        }

        public override void Defeated()
        {
            GameObject.Find("SoloPlayer")
                .transform
                .GetChild(0)
                .GetComponent<Player>().defeatedBoss = true;
            PanelManager.Instance.SwapPanel(Panel.GameWon);
            base.Defeated();
        }
    }
}