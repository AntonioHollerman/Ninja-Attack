using System.Collections.Generic;
using BaseClasses;
using UnityEngine;

namespace Implementations.Techniques
{
    public class FireSummon : Technique
    {
        public float detectDistance = 20;
        protected override void Execute()
        {
            
        }

        private GameObject GetClosestTarget(List<CharacterSheet> targets)
        {
            GameObject target = null;
            float distance = detectDistance;

            foreach (CharacterSheet t in targets)
            {
                float d = Mathf.Abs((transform.position - t.transform.position).magnitude);
                if (d < distance)
                {
                    distance = d;
                    target = t.gameObject;
                }
            }
            return target;
        }
    }
}