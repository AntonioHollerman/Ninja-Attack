using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BaseClasses
{
    /// <summary>
    /// Represents a character's core attributes and behaviors in the game.
    /// </summary>
    public class CharacterSheet : MonoBehaviour
    {
        public bool IsALive => Hp > 0; // True if the character is alive, false otherwise

        // Current stats for the character
        protected int Hp;    // Current health points
        protected int Mana;  // Current mana points

        // Durations for various temporary statuses
        private float _stunDuration; // Duration of stun
        private float _animationBlockedDuration; // Duration of animation block

        // Properties to check if the character is vulnerable, stunned, or has animation blocked
        protected bool IsStunned => _stunDuration > 0;
        protected bool AnimationBlocked => _animationBlockedDuration > 0;

        // Dictionary to store equipped items, and a list to store techniques
        private List<EquippedTechnique> _techniques;
        private List<Effect> _effects;

        // Property to manage the length of the techniques list
        private int _techLen;
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
            public int ManaCost => _tech.ManaCost; // Mana cost of the technique

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
                _tech.Cast(_master); // Cast the technique on the owning character
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
        /// Unity's Start method. Calls the StartWrapper to initialize the character.
        /// </summary>
        void Start()
        {
            StartWrapper(); // Calls a custom initialization method
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
        protected virtual void StartWrapper()
        {

            // Initialize techniques with null values
            _techniques = new List<EquippedTechnique>();
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
        }

        /// <summary>
        /// Deals damage to the character, considering vulnerabilities and defense.
        /// </summary>
        /// <param name="dmg">The amount of damage to deal.</param>
        public void DealDamage(int dmg)
        {
            // Apply damage, reducing health based on vulnerability and defense
            Hp -= dmg;
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
        public bool CastAbility(int position)
        {
            // Ensure the technique exists, there's enough mana, and animations aren't blocked
            bool CanCast()
            {
                return _techniques[position] != null && Mana > _techniques[position].ManaCost && !AnimationBlocked;
            }

            if (!CanCast())
            {
                return false; // Return false if casting conditions weren't met
            }
            
            bool successful = _techniques[position].ActivateTechnique(); // Try to cast the technique
            if (successful)
            {
                Mana -= _techniques[position].ManaCost; // Deduct mana
                _animationBlockedDuration = _techniques[position].AnimationDuration; //
                // Set animation blocked duration based on the technique's animation duration
            }
            return successful; // Return whether the technique was successfully cast
        }

        /// <summary>
        /// Stuns the character for a specific duration.
        /// </summary>
        /// <param name="duration">The duration of the stun effect.</param>
        public void Stun(float duration)
        {
            // Update stun duration to the maximum of the current or new duration
            _stunDuration = duration > _stunDuration ? duration : _stunDuration;
        }
    }
}
