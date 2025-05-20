using System;
using System.Collections;
using BaseClasses;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Implementations.Panels
{
    public class EditKeybindButton : MonoBehaviour
    {
        public Player player;
        public Code workingCode;
        public Image outlineImage;
        public TextMeshProUGUI keybindText;

        private Color _defaultColor;
        private Color _selectedColor = Color.yellow; 

        private void Awake()
        {
            player = GameObject.Find("SoloPlayer")
                .transform
                .GetChild(0)
                .GetComponent<Player>();
            _defaultColor = outlineImage.color;
            keybindText.text = player.GetKeyCode(workingCode) + "";
        }

        private void OnEnable()
        {
            keybindText.text = player.GetKeyCode(workingCode) + "";
        }

        private IEnumerator InputListener()
        {
            outlineImage.color = _selectedColor;
            yield return new WaitUntil(() => Input.anyKeyDown);
            
            foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(key))
                {
                    if (key == KeyCode.Escape)
                    {
                        
                    }
                    player.SetKeyCode(workingCode, key);
                    keybindText.text = key + "";
                    break;
                }
            }

            outlineImage.color = _defaultColor;
        }

        public void OnClick()
        {
            StartCoroutine(InputListener());
        }
    }
}