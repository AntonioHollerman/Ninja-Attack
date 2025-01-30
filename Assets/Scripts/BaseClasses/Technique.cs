using System;
using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;

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
        
        public float animationBlockDuration;
        public CharacterSheet cs;
        public TextMeshProUGUI countDown;
        public SpriteRenderer icon;
        public Sprite active;
        public Sprite notActive;
        
        public int ManaCost { get; private set; } // How much implemented technique cost
        public float CoolDown { get; private set; } // How many seconds is the cooldown
        private float _timer;
        private bool Ready => cs.CurrentMana >= ManaCost && _timer <= 0;

        public abstract void Execute();

        public void ActivateTech()
        {
            if (!Ready)
            {
                return;
            }
            
            bool successful = cs.CastTechnique(ManaCost, animationBlockDuration);
            if (successful)
            {
                _timer = CoolDown;
                Execute();
            }
        }
        protected virtual void UpdateWrapper()
        {
            if (Ready)
            {
                return;
            }

            _timer -= _timer > 0 ? Time.deltaTime : _timer;
            countDown.text = Math.Ceiling(_timer).ToString(CultureInfo.InvariantCulture);
        }

        protected virtual void StartWrapper()
        {
            ManaCost = manaCost;
            CoolDown = coolDown;
            if (countDown != null && icon != null && active != null && notActive != null)
            {
                StartCoroutine(StateListener());
            }
        }

        private IEnumerator StateListener()
        {
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