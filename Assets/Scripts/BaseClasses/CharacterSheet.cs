using System;
using System.Collections;
using System.Collections.Generic;
using Implementations.Animations;
using Implementations.Managers;
using Implementations.Weapons;
using UnityEngine;
using AnimationState = Implementations.Animations.CharacterAnimation.AnimationState;

namespace BaseClasses
{
    /// <summary>
    /// Represents a character's core attributes and behaviors in the game.
    /// </summary>
    public abstract class CharacterSheet : MonoBehaviour
    {
        public static readonly string DeathMarkPath = "prefabs/Extras/DeathMark";
        public static readonly string HitMarkPath = "prefabs/Extras/HitMark";
        public static readonly string SpawnSmokePath = "prefabs/Extras/SpawnSmoke";
        
        public static List<CharacterSheet> CharacterSheets = new List<CharacterSheet>();
        public static bool                 UniversalStopCsUpdateLoop;

        [Header("Game Object Components")] 
        public bool                 disable;
        public bool                 isLarge;
        public Rigidbody            rb;
        public CharacterSheet       lastHit;
        public AnimationManager     body;
        public Transform            pTransform;

        [Header("Character Sheet Stats")]
        public int   level;
        public float baseHp;
        public float baseAtk;
        public int   baseMana;
        public float def;
        public float speed;
        
        [Header("External Objects")]
        public Melee weapon;
        
        private List<Effect>    _effects;
        private float           _stunDuration; // Duration of stun
        private float           _animationBlockedDuration; // Duration of animation block
        private float           _vulnerableDuration;
        
        public float MaxHp { get; private set; }
        public int   MaxMana { get; private set; }
        public float Atk { get; private set; }
        public int   Mana{ get; private set; }  // Current mana 
        public float Hp { get; private set; }    // Current health 
        
        public bool  IsALive => Hp > 0; // True if the character is, false otherwise

        public bool  IsStunned => _stunDuration > 0;
        public bool IsVulnerable => _vulnerableDuration > 0;
        public bool  AnimationBlocked => _animationBlockedDuration > 0;

        private bool _runningDefeated;
        private Collider _collider;
        
        private float  _unmodifiedDef;
        private bool   _storingDamage;
        private float  _storedDamage;

        public abstract List<CharacterSheet> GetAllies();

        public void StartAbsorbingDamage(int newDef)
        {
            _storedDamage = 0;
            _unmodifiedDef = def;
            def = newDef;
            _storingDamage = true;
        }

        public float StopAbsorbingDamage()
        {
            _storingDamage = false;
            def = _unmodifiedDef;
            return _storedDamage;
        }

        public IEnumerator SlowDown(float duration)
        {
            float oldSpeed = speed;
            speed /= 3;
            yield return new WaitForSeconds(duration);
            speed = oldSpeed;
        }
        public virtual void Defeated()
        {
            if (lastHit is Player player)
            {
                player.AddExp(2 + (int) (10 + Math.Log(level) * Math.Pow(level, 2) / 4));
            }

            StartCoroutine(RunHurtAnimation());
        }

        private IEnumerator RunHurtAnimation()
        {
            body.curState = AnimationState.Hurt;
            yield return new WaitForSeconds(body.GetDuration(AnimationState.Hurt));
            
            Vector3 pos = transform.position;
            GameObject prefab = Resources.Load<GameObject>(DeathMarkPath);
            LoopAnimation script = Instantiate(prefab, pos, prefab.transform.rotation).GetComponent<LoopAnimation>();
            script.StartAnimation();
            Destroy(gameObject);
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
        /// <param name="ownership">The character sheet that dealt the damage</param>
        public virtual void DealDamage(float dmg, CharacterSheet ownership)
        {
            lastHit = ownership;
            // Apply damage, reducing health based on vulnerability and defense
            Hp -= IsVulnerable ? GetFinalDamage(dmg, 0) : GetFinalDamage(dmg, def);
            Hp = Hp < 0 ? 0 : Hp;

            if (_storingDamage)
            {
                _storedDamage += GetFinalDamage(dmg, _unmodifiedDef) / 2.0f;
            }
            
            if (!IsALive && !_runningDefeated)
            {
                CharacterSheets.Remove(this);
                _runningDefeated = true;
                weapon?.gameObject.SetActive(false);
                Defeated();
            }
        }

        /// <summary>
        /// Calculates the final damage value after applying defense reduction.
        /// </summary>
        /// <param name="dmg">The initial damage value.</param>
        /// <param name="defense">The defense value to apply to the damage.</param>
        /// <returns>The final damage value after defense.</returns>
        private float GetFinalDamage(float dmg, float defense)
        {
            // Formula to calculate final damage after defense is applied
            return (dmg * (1 / (0.2f * defense + 0.8f)));
        }

        public virtual void RestoreHp(float hp)
        {
            Hp += hp;
            Hp = Hp > MaxHp ? MaxHp : Hp;
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

        public bool AttackWeapon(float animationDuration)
        {
            if (IsStunned || AnimationBlocked)
            {
                return false;
            }

            weapon.Attack();
            BlockAnimation(animationDuration);
            return true;
        }

        public virtual void RestoreMana(int mana)
        {
            Mana += mana;
            Mana = Mana > MaxMana ? MaxMana : Mana;
            if (this is Player player)
            {
                player.statsUI.UpdateFlagged = true;
            }
        }

        public bool CastTechnique(int manaCost)
        {
            if (Mana < manaCost || IsStunned || AnimationBlocked)
            {
                return false;
            }
            
            Mana -= manaCost;
            
            if (this is Player player)
            {
                player.statsUI.UpdateFlagged = true;
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

            if (weapon == null)
            {
                return;
            }
            if (weapon.deactivateOnStun)
            {
                weapon.Deactivate();
            }
        }
        
        /// <summary>
        /// Makes the character vulnerable for a specific duration.
        /// </summary>
        /// <param name="duration">The duration of vulnerability.</param>
        public void Vulnerable(float duration)
        {
            // Update vulnerability duration to the maximum of the current or new duration
            _vulnerableDuration = duration > _vulnerableDuration ? duration : _vulnerableDuration;
        }

        public void BlockAnimation(float duration)
        {
            _animationBlockedDuration = _animationBlockedDuration > duration ? _animationBlockedDuration : duration;
        }

        /// <summary>
        /// Updates the character's stats based on their level and base attributes.
        /// </summary>
        protected void UpdateStats()
        {
            // Calculate current health, mana, and attack based on the character's level
            Hp = MaxHp = (int)(1.3f * Math.Log(level) * baseHp) + baseHp;
            Mana = MaxMana = 50 * (int)(Math.Log(level + 0.5) * (100 / (float) baseMana)) + baseMana;
            Atk = (float) Math.Log(level) * baseAtk + baseAtk;
        } 
        
        /// <summary>
        /// Unity's Start method. Calls the StartWrapper to initialize the character.
        /// </summary>
        void Awake()
        {
            _collider = GetComponent<Collider>();
            AwakeWrapper(); // Calls a custom initialization method
        }

        /// <summary>
        /// Unity's Update method. Calls the UpdateWrapper to handle frame updates.
        /// </summary>
        private void Update()
        {
            if (UniversalStopCsUpdateLoop || disable)
            {
                _collider.enabled = false;
                rb.velocity = Vector3.zero;
                return;
            }

            _collider.enabled = true;
            UpdateWrapper(); // Calls a custom update method each frame
        }

        /// <summary>
        /// Initializes the character's attributes and equipment at the start of the game.
        /// </summary>
        protected virtual void AwakeWrapper()
        {
            _effects = new List<Effect>();
            
            CharacterSheets.Add(this);
            
            UpdateStats();
            StartCoroutine(LevelChange());
        }

        protected virtual IEnumerator LevelChange()
        {
            while (true)
            {
                int lastLevel = level;
                yield return new WaitUntil(() => lastLevel != level);
                UpdateStats();
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
            _vulnerableDuration -= IsVulnerable ? Time.deltaTime : 0f;

            if (IsStunned || UniversalStopCsUpdateLoop)
            {
                rb.velocity = Vector3.zero;
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
            return gameObject.GetHashCode();
        }

        public override bool Equals(object other)
        {
            if (this == null)
            {
                return false;
            }
            if (other is CharacterSheet cs)
            {
                return gameObject == cs.gameObject;
            }

            return false;
        }

        private void LateUpdate()
        {
            if (!IsALive || disable)
            {
                rb.velocity = Vector3.zero;
            }
        }

        public override string ToString()
        {
            return gameObject.name;
        }
    }
}
