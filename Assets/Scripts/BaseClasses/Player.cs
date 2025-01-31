using Implementations.Characters.HostileScripts;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace BaseClasses
{
    public abstract class Player : CharacterSheet
    {
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

        public GameManager gameManager;
        private bool isDefeated = false;

        protected override void UpdateWrapper()
        {
            base.UpdateWrapper();
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
            Rb.velocity = GetDirection() * speed;
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
            if (Input.GetKey(AttackCode))
            {
                AttackWeapon();
            }
        }

        private void TechniqueHandler()
        {
            if (Input.GetKey(FirstTechnique))
            {
                techOne.ActivateTech();
                UpdateMana();
            }

            if (Input.GetKey(SecondTechnique))
            {
                techTwo.ActivateTech();
                UpdateMana();
            }
        }

        public void UpdateHp()
        {
            _hpSlider.fillAmount = (float) CurrentHp /  maxHp;
            _hpText.text = $"{CurrentHp} / {maxHp}";
        }

        public void UpdateMana()
        {
            _manaSlider.fillAmount = (float)CurrentMana / maxMana;
            _manaText.text = $"{CurrentMana} / {maxMana}";
        }

        public override void DealDamage(float dmg)
        {
            base.DealDamage(dmg);
            UpdateHp();
        }

        public override void RestoreHp(int hp)
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
            if (!isDefeated) // Ensure we only process this once
            {
                isDefeated = true; // Mark this player as defeated
                gameManager = FindObjectOfType<GameManager>();

                GameObject hostileSpawner = GameObject.Find("HostileSpawner");
                foreach (Transform trans in hostileSpawner.transform)
                {
                    GameObject go = trans.gameObject;
                    TrackingBehavior script = go.GetComponent<TrackingBehavior>();
                    script?.RemoveTarget(gameObject);
                }

                // Stops game logic
                Time.timeScale = 0;

                base.Defeated();
            }
        }

        public bool IsDefeated()
        {
            return isDefeated;
        }
    }
}