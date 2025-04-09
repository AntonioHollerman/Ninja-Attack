using System.Collections;
using System.Collections.Generic;
using Implementations.Extras;
using UnityEngine;

namespace BaseClasses
{
    public abstract class Hostile : TrackingBehavior
    {
        public static List<Hostile> Hostiles = new List<Hostile>();
        
        public GameObject healthBar;
        public GameObject manaPotion;
        public GameObject hpPotion;
        
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

        public override void DealDamage(float dmg, CharacterSheet ownership)
        {
            base.DealDamage(dmg, ownership);
            _hbScript.UpdateSlider(Hp / MaxHp);
        }
        public override void Defeated()
        {
            Hostiles.Remove(this);
            DropItem();
            base.Defeated();
            Destroy(healthBar);
        }

        private void DropItem()
        {
            float randVal = Random.value;
            if (randVal <= 0.25f)
            {
                GameObject go = Instantiate(hpPotion, transform.position, hpPotion.transform.rotation);
                go.transform.position = new Vector3(go.transform.position.x, go.transform.position.y, 0);
            }

            if (randVal >= 0.75f)
            {
                GameObject go = Instantiate(manaPotion, transform.position, manaPotion.transform.rotation);
                go.transform.position = new Vector3(go.transform.position.x, go.transform.position.y, 0);
            }
        }

        protected override IEnumerator LevelChange()
        {
            while (true)
            {
                int lastLevel = level;
                yield return new WaitUntil(() => lastLevel != level);
                UpdateStats();
                _hbScript.UpdateSlider(Hp / MaxHp);
            }
        }
    }
}