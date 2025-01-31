using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Implementations.Extras;
using UnityEngine;
using UnityEngine.Serialization;

namespace BaseClasses
{
    /// <summary>
    /// Represents a character's core attributes and behaviors in the game.
    /// </summary>
    public abstract class CharacterSheet : MonoBehaviour
    {
        public List<CharacterSheet> allies;
        public float speed;
        public bool IsALive => CurrentHp > 0; // True if the character is alive, false otherwise

        // Current stats for the character
        public float maxHp;
        public float CurrentHp { get; private set; }    // Current health points
        public int maxMana;
        public int CurrentMana{ get; private set; }  // Current mana points

        public static readonly string DeathMarkPath = "prefabs/Extras/DeathMark";
        public static readonly string HitMarkPath = "prefabs/Extras/HitMark";
        

        // Durations for various temporary statuses
        private float _stunDuration; // Duration of stun
        private float _animationBlockedDuration; // Duration of animation block

        // Properties to check if the character is vulnerable, stunned, or has animation blocked
        public bool IsStunned => _stunDuration > 0;
        protected bool AnimationBlocked => _animationBlockedDuration > 0;
        public Rigidbody rb;

        // Dictionary to store equipped items, and a list to store techniques
        private Weapon _weapon;
        private List<Effect> _effects;
        

        public virtual void Defeated()
        {
            Vector3 pos = transform.position;
            Destroy(gameObject);
            
            GameObject prefab = Resources.Load<GameObject>(DeathMarkPath);
            LoopAnimation script = Instantiate(prefab, pos, prefab.transform.rotation).GetComponent<LoopAnimation>();
            script.StartAnimation();
            
        }
        

        
        // Represents an effect applied to a character.
        private class Effect
        {
            // References the character (or "master") to which the effect is applied.
            private readonly CharacterSheet _master;
            // The specific status effect being applied.
            private readonly StatusEffect _se;
            // The time when the effect will stop.
            private readonly float _stop;
            // Coroutine reference to manage the effect's lifecycle.
            private Coroutine _self;

            // Constructor to initialize the effect with the status effect, duration, and the character.
            public Effect(StatusEffect se, float duration, CharacterSheet master)
            {
                _master = master; // Assign the character master.
                _se = se; // Assign the status effect.
                _stop = Time.time + duration; // Calculate the stop time by adding the duration to the current time.
                LoadEffect(); // Load the effect and start its processing.
            }

            // Loads the effect and manages stacking and removal of existing effects.
            private void LoadEffect()
            {
                // Iterate through all active effects on the character.
                foreach (var effect in _master._effects)
                {
                    // Skip if the effect is different from the current one.
                    if (!_se.Equals(effect._se))
                    {
                        continue;
                    }

                    // If the current effect ends later than the existing one, exit early.
                    if (_stop < effect._stop)
                    {
                        return;
                    }
                    
                    // Stop the existing effect's coroutine and remove the effect.
                    _master.StopCoroutine(effect._self);
                    _master._effects.RemoveAll(x => x.Equals(this));
                    break;
                }
                // Start a new coroutine for this effect and add it to the active effects.
                _self = _master.StartCoroutine(EffectLoop());
                _master._effects.Add(this);
            }

            // Coroutine that applies the effect over time until the stop time.
            private IEnumerator EffectLoop()
            {
                // Continue applying the effect until the stop time is reached.
                while (Time.time < _stop)
                {
                    _se(_master, Time.deltaTime); // Apply the status effect on each frame.
                    yield return null; // Wait for the next frame.
                }
                // Remove the effect once it has finished.
                _master._effects.RemoveAll(x => x.Equals(this));
            }

            // Override the Equals method to compare effects by the status effect.
            public override bool Equals(object obj)
            {
                return _se.Equals(obj);
            }

            // Override GetHashCode to use the status effect's hash code.
            public override int GetHashCode()
            {
                return _se.GetHashCode();
            }
        }

        /// <summary>
        /// Deals damage to the character, considering vulnerabilities and defense.
        /// </summary>
        /// <param name="dmg">The amount of damage to deal.</param>
        public virtual void DealDamage(float dmg)
        {
            // Apply damage, reducing health based on vulnerability and defense
            CurrentHp -= dmg;
            CurrentHp = CurrentHp < 0 ? 0 : CurrentHp;
        }

        public virtual void RestoreHp(int hp)
        {
            CurrentHp += hp;
            CurrentHp = CurrentHp > maxHp ? maxHp : CurrentHp;
        }

        /// <summary>
        /// Loads a status effect or updates its duration if already present.
        /// </summary>
        /// <param name="se">The status effect to apply.</param>
        /// <param name="duration">The duration of the status effect.</param>
        public void LoadEffect(StatusEffect se, float duration)
        {
            new Effect(se, duration, this);
        }

        public void EquipWeapon(Weapon w)
        {
            _weapon = w;
        }

        public void AttackWeapon()
        {
            if (IsStunned || AnimationBlocked)
            {
                return;
            }

            if (_weapon.blocksAnimation)
            {
                BlockAnimation(_weapon.GetAnimationBlockDuration());
            }

            IEnumerator PlayAnimation()
            {
                _weapon.gameObject.SetActive(true);
                _weapon.Attack();
                yield return new WaitForSeconds(_weapon.AnimationDuration);
                _weapon.gameObject.SetActive(false);
            }

            StartCoroutine(PlayAnimation());
        }

        public virtual void RestoreMana(int mana)
        {
            CurrentMana += mana;
            CurrentMana = CurrentMana > maxMana ? maxMana : CurrentMana;
            if (this is Player player)
            {
                player.UpdateMana();
            }
        }

        public bool CastTechnique(int manaCost, float animationBlockDuration)
        {
            if (CurrentMana < manaCost || IsStunned || AnimationBlocked)
            {
                return false;
            }

            BlockAnimation(animationBlockDuration);
            CurrentMana -= manaCost;
            
            if (this is Player player)
            {
                player.UpdateMana();
            }
            
            return true;
        }

        /// <summary>
        /// Stuns the character for a specific duration.
        /// </summary>
        /// <param name="duration">The duration of the stun effect.</param>
        public void Stun(float duration)
        {
            // Update stun duration to the maximum of the current or new duration
            _stunDuration = duration > _stunDuration ? duration : _stunDuration;

            if (_weapon == null)
            {
                return;
            }
            if (_weapon.deactivateOnStun)
            {
                _weapon.Deactivate();
            }
        }

        public void BlockAnimation(float duration)
        {
            _animationBlockedDuration = _animationBlockedDuration > duration ? _animationBlockedDuration : duration;
        }

        /// <summary>
        /// Unity's Start method. Calls the StartWrapper to initialize the character.
        /// </summary>
        void Awake()
        {
            AwakeWrapper(); // Calls a custom initialization method
        }

        /// <summary>
        /// Unity's Update method. Calls the UpdateWrapper to handle frame updates.
        /// </summary>
        private void Update()
        {
            UpdateWrapper(); // Calls a custom update method each frame
        }

        /// <summary>
        /// Initializes the character's attributes and equipment at the start of the game.
        /// </summary>
        protected virtual void AwakeWrapper()
        {
            rb = GetComponent<Rigidbody>();
            // Initialize techniques with null values
            CurrentHp = maxHp;
            CurrentMana = maxMana;
            _effects = new List<Effect>();
            
            allies.Add(this);
        }

        /// <summary>
        /// Updates the character's state each frame, applying effects and managing durations.
        /// </summary>
        protected virtual void UpdateWrapper()
        {
            // Reduce the duration of vulnerability, stun, and animation block
            _stunDuration -= IsStunned ? Time.deltaTime : 0f;
            _animationBlockedDuration -= AnimationBlocked ? Time.deltaTime : 0f;
            

            if (!IsALive)
            {
                Defeated();
            }
        }
        
        void OnCollisionEnter(Collision other)
        {
            StopForce(other);
        }

        private void OnCollisionStay(Collision other)
        {
            StopForce(other);
        }

        private void OnCollisionExit(Collision other)
        {
            StopForce(other);
        }

        private void StopForce(Collision other)
        {
            CharacterSheet otherCs = other.gameObject.GetComponent<CharacterSheet>();
            if (otherCs == null)
            {
                return;
            }

            rb.velocity = Vector3.zero;
        }

        public override int GetHashCode()
        {
            return gameObject.name.GetHashCode();
        }

        public override bool Equals(object other)
        {
            if (other is CharacterSheet cs)
            {
                return gameObject.name == cs.gameObject.name;
            }

            return false;
        }

        public override string ToString()
        {
            return gameObject.name;
        }
    }
}
