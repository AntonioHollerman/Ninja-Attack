using System;
using System.Collections;
using System.Collections.Generic;
using Implementations.Animations.CharacterAnimation;
using Implementations.Extras;
using Implementations.Managers;
using Implementations.Panels;
using UnityEngine;
using UnityEngine.Serialization;
using AnimationState = Implementations.Animations.CharacterAnimation.AnimationState;

namespace BaseClasses
{
    public abstract class Player : CharacterSheet
    {
        public static List<Player> Players = new List<Player>();
        public PlayerUI statsUI;
        public GameObject interactIcon;
        
        [Header("Techniques")]
        public Technique techOne;
        public Technique techTwo;
        public List<string> inventory;
        
        [Header("Input Keys")]
        public KeyCode attackCode;
        public KeyCode upCode;
        public KeyCode downCode;
        public KeyCode leftCode;
        public KeyCode rightCode;
        public KeyCode interactCode;

        public KeyCode firstTechniqueCode;
        public KeyCode secondTechniqueCode;

        public bool interacting;
        public bool defeatedBoss;
        public bool InputBlocked => _blockInput > 0 ;
        private float _blockInput;

        public int ExpNeeded {get; private set; }
        public int Exp { get; private set; }

        public Dictionary<TechEnum, Technique> techDict = new Dictionary<TechEnum, Technique>();

        public override List<CharacterSheet> GetAllies()
        {
            return new List<CharacterSheet>(Players);
        }

        protected override void UpdateWrapper()
        {
            base.UpdateWrapper();
            _blockInput -= _blockInput < 0 ? _blockInput : Time.deltaTime;
            if (InputBlocked || IsStunned)
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

            statsUI.UpdateFlagged = true;

            ExpNeeded = CalcExpNeeded();
            Exp = 0;
            StartCoroutine(ExpListener());
            StartCoroutine(InteractListener());
            
            Players.Add(this);

            LoadKeybinds();
        }
        
        private IEnumerator InteractListener()
        {
            while (true)
            {
                yield return new WaitUntil(() => Input.GetKeyDown(interactCode));
                interacting = true;
                yield return new WaitForSeconds(0.3f);
                interacting = false;
            }
        }

        private void HandleDirection()
        {
            Vector3 dir = GetDirection();
            if (dir == Vector3.zero)
            {
                return;
            }
            
            pTransform.localRotation = Quaternion.LookRotation(GetDirection(), Vector3.forward);
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
            if (Input.GetKeyDown(attackCode))
            {
                AttackWeapon(body.GetDuration(AnimationState.Attack));
            }
        }

        private void TechniqueHandler()
        {
            if (Input.GetKeyDown(firstTechniqueCode))
            {
                techOne.ActivateTech();
                statsUI.UpdateFlagged = true;
            }

            if (Input.GetKeyDown(secondTechniqueCode))
            {
                techTwo.ActivateTech();
                statsUI.UpdateFlagged = true;
            }
        }

        public override void DealDamage(float dmg, CharacterSheet ownership)
        {
            base.DealDamage(dmg, ownership);
            statsUI.UpdateFlagged = true;
        }

        public override void RestoreHp(float hp)
        {
            base.RestoreHp(hp);
            statsUI.UpdateFlagged = true;
        }

        public override void RestoreMana(int mana)
        {
            base.RestoreMana(mana);
            statsUI.UpdateFlagged = true;
        }

        public override void Defeated()
        {
            if (PanelManager.Instance.activePanel != Panel.GameWon)
            {
                PanelManager.Instance.SwapPanel(Panel.GameLost);
            }
            foreach (Hostile enemy in Hostile.Hostiles)
            {
                if (enemy == null)
                {
                    continue;
                }
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

        private int CalcExpNeeded()
        {
            return (int) (10 + Math.Log(level) * Math.Pow(level, 2));
        }

        public void AddExp(int exp)
        {
            Exp += exp;
            statsUI.UpdateFlagged = true;
        }
        protected override IEnumerator LevelChange()
        {
            while (true)
            {
                int lastLevel = level;
                yield return new WaitUntil(() => lastLevel != level);
                UpdateStats();
                statsUI.UpdateFlagged = true;
            }
        }

        private IEnumerator ExpListener()
        {
            while (true)
            {
                int tempExpNeeded = ExpNeeded;
                yield return new WaitUntil(() => Exp >= ExpNeeded);
                Exp -= tempExpNeeded;
                level++;
                ExpNeeded = CalcExpNeeded();
                statsUI.UpdateFlagged = true;
            }
        }
        private void LoadKeybinds()
        {
            upCode = GetKeyCode("Key_Up", KeyCode.W);
            downCode = GetKeyCode("Key_Down", KeyCode.S);
            leftCode = GetKeyCode("Key_Left", KeyCode.A);
            rightCode = GetKeyCode("Key_Right", KeyCode.D);
            attackCode = GetKeyCode("Key_Attack", KeyCode.Space);
            firstTechniqueCode = GetKeyCode("Key_Tech1", KeyCode.Q);
            secondTechniqueCode = GetKeyCode("Key_Tech2", KeyCode.E);
        }

        private KeyCode GetKeyCode(string key, KeyCode defaultKey)
        {
            string keyString = PlayerPrefs.GetString(key, defaultKey.ToString());
            return (KeyCode)Enum.Parse(typeof(KeyCode), keyString);
        }

        public void SetKeyCode(Code target, KeyCode newKey)
        {
            switch (target)
            {
                case Code.Up:
                    upCode = newKey;
                    break;
                case Code.Down:
                    downCode = newKey;
                    break;
                case Code.Right:
                    rightCode = newKey;
                    break;
                case Code.Left:
                    leftCode = newKey;
                    break;
                case Code.Atk:
                    attackCode = newKey;
                    break;
                case Code.TechOne:
                    firstTechniqueCode = newKey;
                    break;
                case Code.TechTwo:
                    secondTechniqueCode = newKey;
                    break;
                case Code.Interact:
                    interactCode = newKey;
                    break;
            }
        }

        public KeyCode GetKeyCode(Code target)
        {
            return target switch
            {
            Code.Up      => upCode,
            Code.Down    => downCode,
            Code.Right   => rightCode,
            Code.Left    => leftCode,
            Code.Atk     => attackCode,
            Code.TechOne => firstTechniqueCode,
            Code.TechTwo => secondTechniqueCode,
            Code.Interact => interactCode,
            _ => throw new ArgumentOutOfRangeException(nameof(target), target, null)
            };
        }
    }
}