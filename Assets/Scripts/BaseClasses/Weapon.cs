using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace BaseClasses
{
    public abstract class Weapon : HitBox
    {
        public GameObject parent;
        protected CharacterSheet ParentCs;
        public float AnimationDuration { get; protected set;}
        public bool deactivateOnStun;
        protected List<CharacterSheet> Ignore;
        private Coroutine _animation;
        
        protected abstract IEnumerator PlayAnimation();

        protected override void StartWrapper()
        {
            base.StartWrapper();
            ParentCs = parent.GetComponent<CharacterSheet>();
            ParentCs.EquipWeapon(this);
        }
        
        public void Execute()
        {
            if (_animation != null)
            {
                StopCoroutine(_animation);
            }
            Activate(AnimationDuration, Ignore);
            _animation = StartCoroutine(PlayAnimation());
        }
    }
}