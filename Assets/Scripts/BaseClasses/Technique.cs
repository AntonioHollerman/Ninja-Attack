using System;
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
        
        public CharacterSheet cs;
        public TextMeshProUGUI countDown;
        public SpriteRenderer icon;
        public Sprite active;
        public Sprite notActive;
        
        public int ManaCost { get; private set; } // How much implemented technique cost
        public float CoolDown { get; private set; } // How many seconds is the cooldown
        private float _timer;
        private bool Ready => cs.CurrentMana >= ManaCost && _timer <= 0;

        protected virtual void UpdateWrapper()
        {
            
        }

        protected virtual void StartWrapper()
        {
            ManaCost = manaCost;
            CoolDown = coolDown;
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