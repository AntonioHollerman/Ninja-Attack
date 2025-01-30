using System.Collections.Generic;
using Implementations;
using Implementations.Extras;
using UnityEditor;
using UnityEngine;

namespace BaseClasses
{
    public abstract class Hostile : TrackingBehavior
    {
        public GameObject healthBar;
        private HealthBar _hbScript;
        protected List<CharacterSheet> GetAllies ()
        {
            List<CharacterSheet> alliesLs = new List<CharacterSheet>{this};
            GameObject hostileSpawner = GameObject.Find("HostileSpawner");
            
            foreach (Transform trans in hostileSpawner.transform)
            {
                CharacterSheet ally = trans.GetComponent<CharacterSheet>();
                if (ally != null)
                {
                    alliesLs.Add(ally);
                }
            }

            return alliesLs;
        }

        protected override void AwakeWrapper()
        {
            base.AwakeWrapper();
            healthBar = Instantiate(healthBar, GameObject.Find("Canvas").transform);
            _hbScript = healthBar.GetComponent<HealthBar>();
            _hbScript.target = transform;
        }

        public override void DealDamage(float dmg)
        {
            base.DealDamage(dmg);
            _hbScript.UpdateSlider((float) CurrentHp / maxHp);
        }
        public override void Defeated()
        {
            base.Defeated();
            Destroy(healthBar);
        }
    }
}