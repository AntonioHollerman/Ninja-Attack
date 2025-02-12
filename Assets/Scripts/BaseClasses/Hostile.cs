using System.Collections.Generic;
using Implementations;
using Implementations.Extras;
using UnityEditor;
using UnityEngine;

namespace BaseClasses
{
    public abstract class Hostile : TrackingBehavior
    {
        public static List<Hostile> Hostiles = new List<Hostile>();
        public GameObject healthBar;
        private HealthBar _hbScript;
        protected List<CharacterSheet> GetAllies ()
        {
            return new List<CharacterSheet>(Hostiles);
        }

        protected override void AwakeWrapper()
        {
            base.AwakeWrapper();
            if (healthBar != null)
            {
                healthBar = Instantiate(healthBar, GameObject.Find("Canvas").transform);
                _hbScript = healthBar.GetComponent<HealthBar>();
            }
            _hbScript.target = transform;
            
            GameObject playerOne = GameObject.Find("PlayerOne");
            GameObject playerTwo = GameObject.Find("PlayerTwo");
            
            AddTarget(playerOne);
            AddTarget(playerTwo);
            
            Hostiles.Add(this);
        }

        public override void DealDamage(float dmg)
        {
            base.DealDamage(dmg);
            _hbScript.UpdateSlider(CurrentHp / maxHp);
        }
        public override void Defeated()
        {
            Hostiles.Remove(this);
            base.Defeated();
            Destroy(healthBar);
        }
    }
}