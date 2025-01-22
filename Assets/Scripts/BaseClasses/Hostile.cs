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
                Debug.Log(trans.gameObject.name);
                CharacterSheet ally = trans.GetComponent<CharacterSheet>();
                if (ally != null)
                {
                    Debug.Log($"{trans.gameObject.name}: Made it");
                    allies.Add(ally);
                }
            }

            return allies;
        }
    }
}