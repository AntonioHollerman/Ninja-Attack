using System.Collections.Generic;
using Implementations;
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
            List<CharacterSheet> allies = new List<CharacterSheet>();
            GameObject hostileSpawner = GameObject.Find("HostileSpawner");
            
            foreach (Transform trans in hostileSpawner.transform)
            {
                CharacterSheet ally = trans.GetComponent<CharacterSheet>();
                if (ally != null)
                {
                    allies.Add(ally);
                }
            }

            return allies;
        }

        protected override void AwakeWrapper()
        {
            base.AwakeWrapper();
            healthBar = Instantiate(healthBar, GameObject.Find("Canvas").transform);
            _hbScript = healthBar.GetComponent<HealthBar>();
            _hbScript.target = transform;
        }

        public override void DealDamage(int dmg)
        {
            base.DealDamage(dmg);
            _hbScript.UpdateSlider((float) CurrentHp / maxHp);
        }
    }
}