using System.Collections;
using System.Collections.Generic;
using Implementations.Extras;
using UnityEngine;

namespace BaseClasses
{
    public abstract class Hostile : TrackingBehavior
    {
        public static List<CharacterSheet> Hostiles = new List<CharacterSheet>();
        
        public GameObject healthBar;
        public GameObject manaPotion;
        public GameObject hpPotion;
        
        private HealthBar _hbScript;
        public override List<CharacterSheet> GetAllies ()
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
            _hbScript.target = pTransform;
            
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
            Destroy(healthBar);
            base.Defeated();
        }

        private void DropItem()
        {
            float randVal = Random.value;
            if (randVal <= 0.25f)
            {
                GameObject go = Instantiate(hpPotion, pTransform.position, hpPotion.transform.rotation);
                go.transform.position = new Vector3(go.transform.position.x, go.transform.position.y, 0);
            }

            if (randVal >= 0.75f)
            {
                GameObject go = Instantiate(manaPotion, pTransform.position, manaPotion.transform.rotation);
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