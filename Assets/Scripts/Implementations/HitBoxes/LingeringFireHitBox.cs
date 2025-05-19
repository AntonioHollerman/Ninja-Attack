using System.Collections.Generic;
using BaseClasses;
using UnityEngine;

namespace Implementations.HitBoxes
{
    public class LingeringFireHitBox : MonoBehaviour
    {
        public CharacterSheet parent;

        private void OnTriggerStay(Collider other)
        {
            float burnEffectDps = 0.4f * parent.Atk;
            CharacterSheet cs = other.gameObject.GetComponent<CharacterSheet>();
            if (cs == null)
            {
                return;
            }
            cs.DealDamage(burnEffectDps * Time.deltaTime, parent);
        }
    }
}