using System.Collections;
using UnityEngine;

namespace Implementations.Extras.UniqueAnimation
{
    public class StaticDischargeAnimation : LoopAnimation
    {
        public int startToGrow;
        public float growRate;
        protected override IEnumerator PlayAnimation()
        {
            int i = 0;
            bool growing = false;
            while (true)
            {
                sr.sprite = frames[i];
                i++;
                if (i == frames.Length && loopOnce)
                {
                    Destroy(gameObject);
                }

                if (i == startToGrow)
                {
                    growing = true;
                }
                if (growing)
                {
                    transform.localScale *= growRate;
                }
                if (i == frames.Length)
                {
                    i = 0;
                    transform.localScale = new Vector3(1, 1, 1);
                    growing = false;
                }
                yield return new WaitForSeconds(SecondsBetweenFrame);
            }
        }
    }
}