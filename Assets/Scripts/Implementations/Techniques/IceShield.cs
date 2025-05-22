using System.Collections;
using System.Collections.Generic;
using BaseClasses;
using Implementations.Animations.UniqueAnimation;
using Implementations.HitBoxes;
using UnityEngine;

namespace Implementations.Techniques
{
    public class IceShield : Technique
    {
        public float shieldDuration;
        public float effectDuration;

        public GameObject shieldPrefab;
        public GameObject explosionPrefab;

        private GameObject _shieldInstance;

        private IEnumerator TechListener()
        {
            _shieldInstance = Instantiate(shieldPrefab, parent.transform.parent.position,
                shieldPrefab.transform.rotation);
            IceShieldAnimation ani = _shieldInstance.GetComponent<IceShieldAnimation>();
            ani.durationLooped = shieldDuration;
            ani.GetFrames();
            ani.StartAnimation();
            
            parent.StartAbsorbingDamage(25);
            while (_shieldInstance != null)
            {
                _shieldInstance.transform.position = parent.transform.parent.position;
                yield return null;
            }
            
            GameObject explosionInstance = Instantiate(explosionPrefab, parent.transform.parent.position,
                explosionPrefab.transform.rotation);
            IceHitBox hitBox = explosionInstance.GetComponent<IceHitBox>();
            hitBox.effectDuration = effectDuration;
            hitBox.parent = parent;
            hitBox.absorbedDmg = parent.StopAbsorbingDamage();
            hitBox.Activate(1);

            while (explosionInstance != null)
            {
                explosionInstance.transform.position = parent.transform.position;
                yield return null;
            }
        }
        
        protected override void Execute()
        {
            StartCoroutine(TechListener());
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