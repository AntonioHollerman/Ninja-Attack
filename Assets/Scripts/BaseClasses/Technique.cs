﻿using System;
using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using AnimationState = Implementations.Animations.CharacterAnimation.AnimationState;

namespace BaseClasses
{
    /// <summary>
    /// will run following effects every frame
    /// <param name="cs">The combative character effects will be applied on</param>
    /// <param name="deltaTime">Used to help with applying logic per second</param>
    /// </summary>x
    public delegate void StatusEffect(CharacterSheet cs, float deltaTime);
    
    public abstract class Technique : MonoBehaviour
    {
        [SerializeField] private int manaCost;
        [SerializeField] private float coolDown;
        
        public CharacterSheet parent;
        public Sprite active;
        public Sprite notActive;

        public TextMeshProUGUI countDown;
        public SpriteRenderer icon;
        public GameObject iconGo;
        
        public int ManaCost { get; private set; } // How much implemented technique cost
        public float CoolDown { get; protected set; } // How many seconds is the cooldown
        protected float Timer;
        protected bool Ready => parent != null && parent.Mana >= ManaCost && Timer <= 0;

        protected abstract void Execute();
        public abstract TechEnum GetTechEnum();

        public virtual void ActivateTech()
        {
            if (parent == null)
            {
                Destroy(gameObject);
            }
            if (!Ready)
            {
                return;
            }
            
            bool successful = parent.CastTechnique(ManaCost);
            if (successful)
            {
                parent.body.curState = AnimationState.SpellCast;
                parent.BlockAnimation(parent.body.GetDuration(AnimationState.SpellCast));
                
                Timer = CoolDown;
                Execute();
            }
        }
        protected virtual void UpdateWrapper()
        {
            if (Ready)
            {
                return;
            }

            Timer -= Timer > 0 ? Time.deltaTime : Timer;
        }

        protected virtual void StartWrapper()
        {
            ManaCost = manaCost;
            CoolDown = coolDown;
            if (countDown != null && icon != null && active != null && notActive != null)
            {
                StartCoroutine(StateListener());
                StartCoroutine(UpdateCountDown());
            }
        }

        private IEnumerator UpdateCountDown()
        {
            while (true)
            {
                countDown.text = Math.Ceiling(Timer).ToString(CultureInfo.InvariantCulture);
                yield return null;
            }
        }
        protected virtual IEnumerator StateListener()
        {
            if (!Ready)
            {
                countDown.gameObject.SetActive(true);
                icon.sprite = notActive;
            }
            while (true)
            {
                yield return new WaitUntil(() => Ready);
                countDown.gameObject.SetActive(false);
                icon.sprite = active;
                yield return new WaitUntil(() => !Ready);
                countDown.gameObject.SetActive(true);
                icon.sprite = notActive;
            }
        }

        public void DestroyTech()
        {
            Destroy(icon.gameObject);
            Destroy(countDown.gameObject);
            Destroy(gameObject);
        }

        private void Update()
        {
            UpdateWrapper();
        }

        private void Start()
        {
            StartWrapper();
        }
    }
}