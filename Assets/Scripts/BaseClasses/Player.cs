using UnityEngine;

namespace BaseClasses
{
    public abstract class Player: CharacterSheet
    {
        public float speed;
        
        protected KeyCode UpCode;
        protected KeyCode DownCode;
        protected KeyCode LeftCode;
        protected KeyCode RightCode;

        private Vector3 _lastPos;

        protected override void UpdateWrapper()
        {
            base.UpdateWrapper();
            HandleMovement();
            HandleDirection();
            _lastPos = transform.position;
        }

        protected override void StartWrapper()
        {
            base.StartWrapper();
            _lastPos = transform.position;
        }

        private void HandleDirection()
        {
            Vector3 lastVec = _lastPos;
            Vector3 curVec = transform.position;
            

            // Calculate the direction vector
            Vector3 direction = lastVec - curVec;

            // Ensure the direction vector is not zero
            if (direction != Vector3.zero)
            {
                // Calculate the rotation to face the target
                Quaternion targetRotation = Quaternion.LookRotation(direction);

                // Apply the rotation globally
                transform.rotation = targetRotation;
            }
            
        }

        private void HandleMovement()
        {
            if (Input.GetKey(UpCode))
            {
                transform.Translate(Time.deltaTime * speed * Vector3.up);
            }

            if (Input.GetKey(DownCode))
            {
                transform.Translate(Time.deltaTime * speed * Vector3.down);
            }

            if (Input.GetKey(LeftCode))
            {
                transform.Translate(Time.deltaTime * speed * Vector3.left);
            }

            if (Input.GetKey(RightCode))
            {
                transform.Translate(Time.deltaTime * speed * Vector3.right);
            }
        }
    }
}