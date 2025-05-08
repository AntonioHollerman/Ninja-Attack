using System;
using BaseClasses;
using UnityEngine;

namespace Implementations.Extras
{
    public class TeleportPair : MonoBehaviour
    {
        public TeleportPair otherPair;

        private Player _player;
        private bool _ignore;
        private float _yOffset;

        private void Awake()
        {
            _player = GameObject.Find("SoloPlayer").GetComponent<Player>();
            _yOffset = 0.2f;
        }
        private void OnTriggerEnter(Collider other)
        {
            _player = other.gameObject.GetComponent<Player>();
            _ignore = _player == null;
        }

        private void OnTriggerStay(Collider other)
        {
            if (!_ignore && _player.interacting)
            {
                _player.interacting = false;
                _player.transform.position = otherPair.transform.position + Vector3.up * _yOffset;
            }
        }
    }
}