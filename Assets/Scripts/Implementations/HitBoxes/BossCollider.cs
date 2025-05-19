using System;
using BaseClasses;
using UnityEngine;

namespace Implementations.HitBoxes
{
    public class BossCollider : MonoBehaviour
    {
        public CharacterSheet boss;

        private void OnTriggerEnter(Collider other)
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                boss.disable = false;
                GetComponent<Collider>().enabled = false;
            }
        }
    }
}