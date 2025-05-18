using System;
using System.Collections;
using System.Collections.Generic;
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

        private List<string> _toDisplay = new List<string>(); 
        private void Awake()
        {
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
        }

        public void AddMessage(string message)
        {
            if (!_toDisplay.Contains(message))
            {
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

                yield return new WaitForSeconds(2.5f);
                
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