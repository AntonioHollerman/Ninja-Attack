using System;
using BaseClasses;
using UnityEngine;
using Implementations.Extras; // make sure to include the namespace for DungeonLevelType if in another namespace

namespace Implementations.Teleporting
{
    public class TeleportPair : MonoBehaviour
    {
        public TeleportPair otherPair;
        private float _yOffset;

        // New public field to specify destination type for musi

        private void Awake()
        {
            _yOffset = 0.2f;
        }

        private void OnTriggerEnter(Collider other)
        {
            Player p = other.GetComponent<Player>();
            if (p != null)
            {
                p.interactIcon.SetActive(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            Player p = other.GetComponent<Player>();
            if (p != null)
            {
                p.interactIcon.SetActive(false);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            Player p = other.GetComponent<Player>();
            if (p != null && p.interacting && ConditionMet(p))
            {
                p.interacting = false;
                p.pTransform.position = otherPair.transform.position + Vector3.up * _yOffset;

             
            }
        }

        public virtual bool ConditionMet(Player player)
        {
            return true;
        }
    }
}
