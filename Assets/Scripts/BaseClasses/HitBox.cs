using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace BaseClasses
{
    public abstract class HitBox : MonoBehaviour
    {
        public bool blocksAnimation;
        public bool destroyOnFinish;
        // List of characters to ignore for the hit detection
        public List<CharacterSheet> ignore;
        
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
                gameObject.SetActive(true);
                yield return new WaitUntil((() => !IsAlive));
                if (destroyOnFinish)
                {
                    Destroy(gameObject);
                    break;
                }
                gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Unity's OnTriggerEnter method. Called when another collider enters the trigger collider.
        /// </summary>
        private void OnTriggerEnter(Collider other)
        {
            // Attempt to get the CharacterSheet component from the collided object
            CharacterSheet cs = other.gameObject.GetComponent<CharacterSheet>();
            
            // If no CharacterSheet component is found, or if the character is in the ignore list, exit the method
            if (cs == null || ignore.Contains(cs))
            {
                return;
            }

            // Apply the effect to the character and add them to the ignore list
            Effect(cs);
            ignore.Add(cs);
        }
        
        protected virtual void StartWrapper()
        {
            StartCoroutine(AliveChecker());
            ignore = new List<CharacterSheet>();
        }

        protected virtual void UpdateWrapper()
        {
            // Decrease the alive time every frame
            _aliveTime -= Time.deltaTime;
        }
        
        private void Start()
        {
            StartWrapper();
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
