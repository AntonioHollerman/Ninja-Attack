using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseClasses
{
    public abstract class HitBox : MonoBehaviour
    {
        public CharacterSheet parent;
        public bool destroyOnFinish;
        public bool workOnCollider;
        public bool dontWorkOnTimer;

        protected Collider Collider;
        protected List<CharacterSheet> ActiveIgnore = new List<CharacterSheet>();
        private float _aliveTime;
        private bool IsAlive => _aliveTime > 0;

        /// <summary>
        /// Abstract method to apply effects to the character when the hitbox interacts with it.
        /// Must be implemented by derived classes.
        /// </summary>
        protected abstract void Effect(CharacterSheet cs);

        public void Activate(float duration)
        {
            IEnumerator WaitForParent()
            {
                yield return new WaitUntil(() => parent != null);
                _aliveTime = duration;
            }

            ActiveIgnore = parent.GetAllies();
            StartCoroutine(WaitForParent());
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
                if (workOnCollider)
                {
                    Collider.enabled = true;
                }
                yield return new WaitUntil(() => !IsAlive);
                if (workOnCollider)
                {
                    Collider.enabled = false;
                }
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
            if (cs == null || ActiveIgnore.Contains(cs))
            {
                return;
            }

            // Apply the effect to the character and add them to the ignore list
            Effect(cs);
            ActiveIgnore.Add(cs);
        }

        private void OnTriggerStay(Collider other)
        {
            TriggerStayWrapper(other);
        }
        
        protected virtual void TriggerEnterWrapper(Collider other){}   
        protected virtual void TriggerStayWrapper(Collider other){}
        protected virtual void AwakeWrapper()
        {
            if (!dontWorkOnTimer)
            {
                StartCoroutine(AliveChecker());
            }
            
            if (workOnCollider)
            {
                Collider = gameObject.GetComponent<Collider>();
                Collider.enabled = false;
            }
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
