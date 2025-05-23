﻿using System.Collections;
using BaseClasses;
using UnityEngine;
using AnimationState = Implementations.Animations.CharacterAnimation.AnimationState;

namespace Implementations.Characters.HostileScripts
{
    public class Brawler : Hostile
    {
        public float atkDistance;

        protected override void UpdateWrapper()
        {
            base.UpdateWrapper();
            if (IsStunned || UniversalStopCsUpdateLoop)
            {
                return;
            }
            
            rb.velocity = pTransform.forward * speed;
            if (TargetDistance <= atkDistance && IsALive)
            {
                AttackWeapon(body.GetDuration(AnimationState.Attack));
            }
        }

        protected override void AwakeWrapper()
        {
            base.AwakeWrapper();
            AddTarget(GameObject.Find("SoloPlayer"));
        }
    }
}