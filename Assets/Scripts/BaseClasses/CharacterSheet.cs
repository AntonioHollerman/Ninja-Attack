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
        protected bool IsStunned => _stunDuration > 0;
        protected bool AnimationBlocked => _animationBlockedDuration > 0;
        protected Rigidbody Rb;

        // Dictionary to store equipped items, and a list to store techniques
        private Weapon _weapon;
        private List<EquippedTechnique> _techniques;
        private List<Effect> _effects;

        // Property to manage the length of the techniques list
        private int _techLen;

        public virtual void Defeated()
        {
            Vector3 pos = transform.position;
            Destroy(gameObject);
            GameObject prefab = Resources.Load<GameObject>(DeathMarkPath);
            LoopAnimation script = Instantiate(prefab, pos, prefab.transform.rotation).GetComponent<LoopAnimation>();
            script.StartAnimation();
        }
        
        public int TechniquesLength
        {
            get => _techLen;
            set
            {
                if (value > _techLen) // Expand the list if new length is greater
                {
                    for (int i = 0; i < value - _techLen; i++)
                    {
                        _techniques.Add(null); // Add null to represent empty technique slots
                    }
                }
                else // Truncate the list if new length is smaller
                {
                    _techniques = _techniques.Take(value).ToList(); // Reduce the list size
                }
                _techLen = value; // Update the stored length value
            }
        }

        /// <summary>
        /// Represents an equipped technique and its associated state.
        /// </summary>
        private class EquippedTechnique
        {
            private readonly CharacterSheet _master; // Reference to the character that owns this technique
            private readonly Technique _tech; // The technique itself
            private float _cooldown; // Current cooldown of the technique
           
            private bool OnCooldown => _cooldown > 0; // True if the technique is on cooldown
            public float AnimationDuration => _tech.AnimationDuration; // Duration of the technique's animation
            public int ManaCost => _tech.ManaCost; // CurrentMana cost of the technique

            /// <summary>
            /// Constructor to initialize the equipped technique.
            /// </summary>
            /// <param name="tech">The technique to equip.</param>
            /// <param name="master">The character who owns this technique.</param>
            public EquippedTechnique(Technique tech, CharacterSheet master)
            {
                _tech = tech; // Store the technique
                _cooldown = 0; // Start with no cooldown
                _master = master; // Store reference to the character
            }

            /// <summary>
            /// Decreases the cooldown each frame, if applicable.
            /// </summary>
            public void DecrementCount()
            {
                _cooldown -= OnCooldown ? Time.deltaTime : 0; // Decrease cooldown by delta time
            }

            /// <summary>
            /// Attempts to activate the technique, returning true if successful.
            /// </summary>
            /// <returns>True if the technique was activated, false otherwise.</returns>
            public bool ActivateTechnique()
            {
                if (OnCooldown) // If the technique is on cooldown, do nothing
                {
                    return false;
                }

                _cooldown = _tech.CoolDown; // Reset the cooldown to the technique's cooldown time
                _tech.Attack(); // Cast the technique on the owning character
                return true; // Technique was successfully activated
            }

            // Compare via technique
            public override bool Equals(object obj)
            {
                return _tech.Equals(obj);
            }

            // Hashcode equals the technique
            public override int GetHashCode()
            {
                return _tech.GetHashCode();
            }
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
        public virtual void DealDamage(int dmg)
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
        
        /// <summary>
        /// Loads a technique into a specific slot if not already present.
        /// </summary>
        /// <param name="tech">The technique to load.</param>
        /// <param name="position">The slot to load the technique into.</param>
        /// <returns>True if the technique was successfully loaded, otherwise false.</returns>
        public bool LoadTechnique(Technique tech, int position)
        {
            EquippedTechnique et = new EquippedTechnique(tech, this); // Create an equipped technique
            if (_techniques.Contains(et)) // Check if the technique is already loaded
            {
                return false; // Technique was already present
            }
            _techniques[position] = et; // Load the technique into the specified position
            return true;
        }

        /// <summary>
        /// Removes a technique from a specific slot.
        /// </summary>
        /// <param name="position">The slot to remove the technique from.</param>
        public void RemoveTechnique(int position)
        {
            _techniques[position] = null; // Set the technique slot to null
        }

        /// <summary>
        /// Casts a technique from a specific slot if enough mana is available.
        /// </summary>
        /// <param name="position">The slot to cast the technique from.</param>
        /// <returns>True if the technique was successfully cast, otherwise false.</returns>
        public virtual bool CastAbility(int position)
        {
            // Ensure the technique exists, there's enough mana, and animations aren't blocked
            bool CanCast()
            {
                return _techniques[position] != null && CurrentMana > _techniques[position].ManaCost && !AnimationBlocked;
            }

            if (!CanCast())
            {
                return false; // Return false if casting conditions weren't met
            }
            
            bool successful = _techniques[position].ActivateTechnique(); // Try to cast the technique
            if (successful)
            {
                CurrentMana -= _techniques[position].ManaCost; // Deduct mana
                _animationBlockedDuration = _techniques[position].AnimationDuration; // Set animation blocked duration based on the technique's animation duration
            }
            return successful; // Return whether the technique was successfully cast
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

        /// <summary>
        /// Stuns the character for a specific duration.
        /// </summary>
        /// <param name="duration">The duration of the stun effect.</param>
        public void Stun(float duration)
        {
            // Update stun duration to the maximum of the current or new duration
            _stunDuration = duration > _stunDuration ? duration : _stunDuration;

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
            Rb = GetComponent<Rigidbody>();
            // Initialize techniques with null values
            CurrentHp = maxHp;
            CurrentMana = maxMana;
            _techniques = new List<EquippedTechnique>();
            _effects = new List<Effect>();
            
            allies.Add(this);
            
            for (int i = 0; i < _techLen; i++)
            {
                _techniques.Add(null); // Fill the list with null values
            }
        }

        /// <summary>
        /// Updates the character's state each frame, applying effects and managing durations.
        /// </summary>
        protected virtual void UpdateWrapper()
        {
            // Reduce the duration of vulnerability, stun, and animation block
            _stunDuration -= IsStunned ? Time.deltaTime : 0f;
            _animationBlockedDuration -= AnimationBlocked ? Time.deltaTime : 0f;

            // Decrement the cooldown of all techniques
            foreach (var tech in _techniques)
            {
                tech?.DecrementCount(); // Decrease cooldown if the technique exists
            }

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

            Rb.velocity = Vector3.zero;
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
