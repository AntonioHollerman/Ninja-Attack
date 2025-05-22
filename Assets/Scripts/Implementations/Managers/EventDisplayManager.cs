using System;
using System.Collections;
using System.Collections.Generic;
using BaseClasses;
using TMPro;
using UnityEngine;

namespace Implementations.Managers
{
    public class EventDisplayManager : MonoBehaviour
    {
        public static EventDisplayManager Instance { get; private set; }
        public SpriteRenderer textBg;
        public TextMeshPro text;
        public float changePerSecond;

        private Player _player;
        private float _duration;
        
        private List<string> _toDisplay = new List<string>(); 
        private void Awake()
        {
            _player = GameObject.Find("SoloPlayer")
                .transform
                .GetChild(0)
                .GetComponent<Player>();
            
            Instance = this;
            text.color = new Color(
                text.color.r,
                text.color.g,
                text.color.b,
                0);
            
            textBg.color = new Color(
                textBg.color.r,
                textBg.color.g,
                textBg.color.b,
                0);
            
            
            StartCoroutine(DisplayListener());
            StartCoroutine(WaitForLevel());
        }

        private IEnumerator WaitForLevel()
        {
            yield return new WaitUntil(() => _player.level >= 5);
            Instance.AddMessage("Ice Shield unlocked! Open 'technique' in pause menu to equip it!", 5);
            
            yield return new WaitUntil(() => _player.level >= 8);
            Instance.AddMessage("Fire ball unlocked! Open 'technique' in pause menu to equip it!", 5);

            yield return new WaitUntil(() => _player.level >= 10);
            Instance.AddMessage("Static unlocked! Open 'technique' in pause menu to equip it!", 5);
        }

        public void AddMessage(string message, float duration = 2.5f)
        {
            if (!_toDisplay.Contains(message))
            {
                _duration = duration;
                _toDisplay.Add(message);
            }
        }
        private IEnumerator DisplayListener()
        {
            while (true)
            {
                yield return new WaitUntil(() => _toDisplay.Count > 0);

                text.text = _toDisplay[0];
                
                // Fade in
                float a = 0.0f;
                while (a < 1)
                {
                    a += changePerSecond * Time.deltaTime;
                    if (a > 1)
                    {
                        a = 1;
                    }
                
                    Color textColor = text.color;
                    Color textBgColor = textBg.color;
                    
                    textColor.a = a;
                    textBgColor.a = a;
                    
                    text.color = textColor;
                    textBg.color = textBgColor;
                    yield return null;
                }

                yield return new WaitForSeconds(_duration);
                
                // Fade out
                a = 1f;
                while (a > 0)
                {
                    a -= changePerSecond * Time.deltaTime;
                    if (a < 0)
                    {
                        a = 0;
                    }
                
                    Color textColor = text.color;
                    Color textBgColor = textBg.color;
                    
                    textColor.a = a;
                    textBgColor.a = a;
                    
                    text.color = textColor;
                    textBg.color = textBgColor;
                
                    yield return null;
                }
                _toDisplay.RemoveAt(0);
            }
        }
    }
}