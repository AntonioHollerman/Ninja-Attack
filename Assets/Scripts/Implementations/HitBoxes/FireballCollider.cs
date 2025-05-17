using BaseClasses;
using Implementations.Techniques;
using UnityEngine;

namespace Implementations.HitBoxes
{
    public class FireballCollider : MonoBehaviour
    {
        public Fireball parentTech;
        public Vector3 speed;
        private Rigidbody _rb;

        private void TurnOffForce(Collision other)
        {
            CharacterSheet otherCs = other.gameObject.GetComponent<CharacterSheet>();
            if (otherCs != null)
            {
                otherCs.rb.velocity = Vector3.zero;
                _rb.velocity = speed;
            }
        }
        private void OnCollisionEnter(Collision other)
        {
            TurnOffForce(other);
            
            if (other.gameObject == parentTech.parent.gameObject)
            {
                return;
            }
            parentTech.ActivateTech();
        }

        private void OnCollisionStay(Collision other)
        {
            TurnOffForce(other);
        }

        private void OnCollisionExit(Collision other)
        {
            TurnOffForce(other);
        }

        private void Awake()
        {
            _rb = gameObject.GetComponent<Rigidbody>();
        }
    }
}