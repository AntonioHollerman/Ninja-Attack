﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BaseClasses
{
    public abstract class Player : CharacterSheet
    {
        public static List<Player> Players = new List<Player>();
        public GameObject body;
        public GameObject weaponGo;
        
        public GameObject hpSliderGo;
        public GameObject hpTextGo;
        
        public GameObject manaSliderGo;
        public GameObject manaTextGo;

        public Technique techOne;
        public Technique techTwo;

        protected KeyCode AttackCode;
        
        public KeyCode upCode;
        public KeyCode downCode;
        public KeyCode leftCode;
        public KeyCode rightCode;

        protected KeyCode FirstTechnique;
        protected KeyCode SecondTechnique;
        
        private Image _hpSlider;
        private TextMeshProUGUI _hpText;

        private Image _manaSlider;
        private TextMeshProUGUI _manaText;

        public bool InputBlocked => _blockInput > 0 ;
        private float _blockInput;

        
        protected override void UpdateWrapper()
        {
            base.UpdateWrapper();
            _blockInput -= _blockInput < 0 ? _blockInput : Time.deltaTime;
            if (InputBlocked)
            {
                return;
            }
            HandleMovement();
            HandleDirection();
            AttackHandler();
            TechniqueHandler();
        }

        protected override void AwakeWrapper()
        {
            base.AwakeWrapper();
            
            _hpSlider = hpSliderGo.GetComponent<Image>();
            _hpText = hpTextGo.GetComponent<TextMeshProUGUI>();

            _manaSlider = manaSliderGo.GetComponent<Image>();
            _manaText = manaTextGo.GetComponent<TextMeshProUGUI>();
            
            EquipWeapon(weaponGo.GetComponent<Weapon>());
            
            UpdateHp();
            UpdateMana();
            
            Players.Add(this);
        }

        private void HandleDirection()
        {
            Vector3 dir = GetDirection();
            if (dir == Vector3.zero)
            {
                return;
            }
            
            transform.localRotation = Quaternion.LookRotation(GetDirection());
            
            if (Input.GetKey(leftCode) || Input.GetKey(rightCode))
            {
                body.transform.localRotation = Quaternion.Euler(0, -90, -90);
                weaponGo.transform.localRotation = Quaternion.Euler(0, 0, 90);
                return;
            }

            if (Input.GetKey(upCode) || Input.GetKey(downCode))
            {
                body.transform.localRotation = Quaternion.Euler(90, -90, -90);
                weaponGo.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
        }

        private void HandleMovement()
        {
            rb.velocity = GetDirection() * speed;
        }

        private Vector3 GetDirection()
        {
            Vector3 direction = Vector3.zero;
            if (Input.GetKey(upCode))
            {
                direction += Vector3.up;
            }

            if (Input.GetKey(downCode))
            {
                direction += Vector3.down;
            }

            if (Input.GetKey(leftCode))
            {
                direction += Vector3.left;
            }

            if (Input.GetKey(rightCode))
            {
                direction += Vector3.right;
            }

            return direction.normalized;
        }

        private void AttackHandler()
        {
            if (Input.GetKeyDown(AttackCode))
            {
                AttackWeapon();
            }
        }

        private void TechniqueHandler()
        {
            if (Input.GetKeyDown(FirstTechnique))
            {
                techOne.ActivateTech();
                UpdateMana();
            }

            if (Input.GetKeyDown(SecondTechnique))
            {
                techTwo.ActivateTech();
                UpdateMana();
            }
        }

        public void UpdateHp()
        {
            _hpSlider.fillAmount = Hp /  MaxHp;
            _hpText.text = $"{Math.Floor(Hp)} / {MaxHp}";
        }

        public void UpdateMana()
        {
            _manaSlider.fillAmount = (float) Mana / MaxMana;
            _manaText.text = $"{Mana} / {MaxMana}";
        }

        public override void DealDamage(float dmg, CharacterSheet ownership)
        {
            base.DealDamage(dmg, ownership);
            UpdateHp();
        }

        public override void RestoreHp(float hp)
        {
            base.RestoreHp(hp);
            UpdateHp();
        }

        public override void RestoreMana(int mana)
        {
            base.RestoreMana(mana);
            UpdateMana();
        }

        public override void Defeated()
        {
            foreach (Hostile enemy in Hostile.Hostiles)
            {
                GameObject go = enemy.gameObject;
                TrackingBehavior script = go.GetComponent<TrackingBehavior>();
                script?.RemoveTarget(gameObject);
            }

            Players.Remove(this);
            base.Defeated();
        }

        public void BlockInput(float duration)
        {
            _blockInput = _blockInput > duration ? _blockInput : duration;
            rb.velocity = Vector3.zero;
        }

        protected override IEnumerator LevelChange()
        {
            int lastLevel = level;
            while (true)
            {
                yield return new WaitUntil(() => lastLevel != level);
                UpdateStats();
                UpdateHp();
                UpdateMana();
            }
        }
    }
}