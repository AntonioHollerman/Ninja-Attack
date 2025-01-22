using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace BaseClasses
{
    public abstract class Weapon : HitBox
    {
        public float AnimationDuration { get; protected set;}
        public bool deactivateOnStun;
        private Coroutine _animation;
        
        protected abstract IEnumerator PlayAnimation();
        
        
        public void Execute()
        {
            if (_animation != null)
            {
                StopCoroutine(_animation);
            }
            Activate(AnimationDuration);
            _animation = StartCoroutine(PlayAnimation());
        }
    }
}