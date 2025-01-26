using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace BaseClasses
{
    public abstract class HitBox : MonoBehaviour
    {
        public CharacterSheet parent;
        public bool destroyOnFinish;
        // List of characters to ignore for the hit detection
        private List<CharacterSheet> _activeIgnore;
        
        // Time remaining before the hitbox is destroyed
        private float _aliveTime;
        
        // Property to check if the hitbox is still active (alive time is greater than 0)
        private bool IsAlive => _aliveTime > 0;

        /// <summary>
        /// Abstract method to apply effects to the character when the hitbox interacts with it.
        /// Must be implemented by derived classes.
        /// </summary>
        protected abstract void Effect(CharacterSheet cs);

        public void Activate(float duration)
        {
            _aliveTime = duration;
            _activeIgnore = new List<CharacterSheet>(parent.allies);
        }

        public void Deactivate()
        {
            _aliveTime = 0;
        }

        IEnumerator AliveChecker()
        {
            while (true)
            {
                yield return new WaitUntil(() => IsAlive);
                yield return new WaitUntil(() => !IsAlive);
                if (destroyOnFinish)
                {
                    Destroy(gameObject);
                    break;
                }
            }
        }

        /// <summary>
        /// Unity's OnTriggerEnter method. Called when another collider enters the trigger collider.
        /// </summary>
        private void OnTriggerEnter(Collider other)
        {
            TriggerEnterWrapper(other);
            // Attempt to get the CharacterSheet component from the collided object
            CharacterSheet cs = other.gameObject.GetComponent<CharacterSheet>();
            
            // If no CharacterSheet component is found, or if the character is in the ignore list, exit the method
            if (cs == null || _activeIgnore.Contains(cs))
            {
                return;
            }

            // Apply the effect to the character and add them to the ignore list
            Effect(cs);
            _activeIgnore.Add(cs);
        }

        protected virtual void TriggerEnterWrapper(Collider other){}
        protected virtual void AwakeWrapper()
        {
            StartCoroutine(AliveChecker());
        }

        protected virtual void UpdateWrapper()
        {
            // Decrease the alive time every frame
            _aliveTime -= Time.deltaTime;
        }
        
        private void Awake()
        {
            AwakeWrapper();
        }

        /// <summary>
        /// Unity's Update method. Calls the UpdateWrapper to handle frame updates.
        /// </summary>
        private void Update()
        {
            UpdateWrapper();
        }
    }
}
