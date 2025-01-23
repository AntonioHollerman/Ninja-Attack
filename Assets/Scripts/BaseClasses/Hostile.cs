using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BaseClasses
{
    public abstract class Hostile : TrackingBehavior
    {
        protected List<CharacterSheet> GetAllies ()
        {
            List<CharacterSheet> allies = new List<CharacterSheet>();
            GameObject hostileSpawner = GameObject.Find("HostileSpawner");
            
            foreach (Transform trans in hostileSpawner.transform)
            {
                CharacterSheet ally = trans.GetComponent<CharacterSheet>();
                if (ally != null)
                {
                    allies.Add(ally);
                }
            }

            return allies;
        }
    }
}