using System.Collections;
using UnityEngine;

namespace Implementations.Extras
{
    public class FadeScreen : MonoBehaviour
    {
        public float changePerSecond;
        public SpriteRenderer sr;
        public float Opacity => sr.color.a;
        
        public IEnumerator FadeIn()
        {
            float a = 0.0f;
            while (a < 1)
            {
                a += changePerSecond * Time.deltaTime;
                if (a > 1)
                {
                    a = 1;
                }
                
                Color newColor = sr.color;
                newColor.a = a;
                sr.color = newColor;
                
                yield return null;
            }
        }

        public IEnumerator FadeOut()
        {
            float a = 1f;
            while (a > 0)
            {
                a -= changePerSecond * Time.deltaTime;
                if (a < 0)
                {
                    a = 0;
                }
                
                Color newColor = sr.color;
                newColor.a = a;
                sr.color = newColor;
                
                yield return null;
            }
        }
    }
}