using System;
using BaseClasses;
using UnityEngine;
using Implementations.Extras; // make sure to include the namespace for DungeonLevelType if in another namespace

namespace Implementations.Extras
{
    public class TeleportPair : MonoBehaviour
    {
        public TeleportPair otherPair;
        private float _yOffset;

        // New public field to specify destination type for music
        public DungeonLevelType destinationLevelType;

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
            if (p != null && p.interacting)
            {
                p.interacting = false;
                p.pTransform.position = otherPair.transform.position + Vector3.up * _yOffset;

                // Change music according to destination level type
                if (MusicManager.Instance != null)
                {
                    MusicManager.Instance.ChangeMusic(otherPair.destinationLevelType);
                }
            }
        }
    }
}
