using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BaseClasses
{
    public abstract class Player : CharacterSheet
    {
        public GameObject body;
        
        public GameObject hpSliderGo;
        public GameObject hpTextGo;
        
        public GameObject manaSliderGo;
        public GameObject manaTextGo;

        protected KeyCode UpCode;
        protected KeyCode DownCode;
        protected KeyCode LeftCode;
        protected KeyCode RightCode;

        private Vector3 _lastPos;

        private Image _hpSlider;
        private TextMeshProUGUI _hpText;

        private Image _manaSlider;
        private TextMeshProUGUI _manaText;

        protected override void UpdateWrapper()
        {
            base.UpdateWrapper();
            HandleMovement();
            HandleDirection();
        }

        protected override void StartWrapper()
        {
            base.StartWrapper();
            _lastPos = transform.position;
            
            _hpSlider = hpSliderGo.GetComponent<Image>();
            _hpText = hpTextGo.GetComponent<TextMeshProUGUI>();

            _manaSlider = manaSliderGo.GetComponent<Image>();
            _manaText = manaTextGo.GetComponent<TextMeshProUGUI>();
            
            UpdateHp();
            UpdateMana();
        }

        private void HandleDirection()
        {
            Vector3 lastVec = _lastPos;
            Vector3 curVec = transform.position;


            // Calculate the direction vector
            Vector3 direction = curVec - lastVec;

            // Ensure the direction vector is not zero
            if (direction != Vector3.zero)
            {
                // Calculate the rotation to face the target
                Quaternion targetRotation = Quaternion.LookRotation(direction);

                // Apply the rotation globally
                transform.rotation = targetRotation;
            }
            _lastPos = transform.position;

            if (Input.GetKey(LeftCode) || Input.GetKey(RightCode))
            {
                body.transform.localRotation = Quaternion.Euler(0, -90, -90);
                return;
            }

            if (Input.GetKey(UpCode) || Input.GetKey(DownCode))
            {
                body.transform.localRotation = Quaternion.Euler(90, -90, -90);
            }
        }

        private void HandleMovement()
        {
            if (Input.GetKey(UpCode))
            {
                transform.position += Time.deltaTime * speed * Vector3.up;
            }

            if (Input.GetKey(DownCode))
            {
                transform.position += Time.deltaTime * speed * Vector3.down;
            }

            if (Input.GetKey(LeftCode))
            {
                transform.position += Time.deltaTime * speed * Vector3.left;
            }

            if (Input.GetKey(RightCode))
            {
                transform.position += Time.deltaTime * speed * Vector3.right;
            }
        }

        public void UpdateHp()
        {
            _hpSlider.fillAmount = (float) CurrentHp /  maxHp;
            _hpText.text = $"{CurrentHp} / {maxHp}";
        }

        public void UpdateMana()
        {
            _manaSlider.fillAmount = (float)CurrentMana / maxMana;
            _manaText.text = $"{CurrentMana} / {maxMana}";
        }
    }
}