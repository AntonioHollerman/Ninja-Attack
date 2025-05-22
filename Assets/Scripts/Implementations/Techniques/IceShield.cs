using System.Collections;
using System.Collections.Generic;
using BaseClasses;
using UnityEngine;

namespace Implementations.Techniques
{
    public class IceShield : Technique
    {
        public float shieldDuration;

        public GameObject shieldPrefab;
        public GameObject explosionPrefab;
        protected override void Execute()
        {
            throw new System.NotImplementedException();
        }

        public override TechEnum GetTechEnum()
        {
            return TechEnum.IceShield;
        }
        
        protected override void StartWrapper()
        {
            base.StartWrapper();
            CoolDown += shieldDuration;
        }
    }
}