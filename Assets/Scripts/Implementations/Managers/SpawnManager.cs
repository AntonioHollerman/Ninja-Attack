using System;
using System.Collections.Generic;
using BaseClasses;
using Implementations.Extras;
using UnityEngine;

namespace Implementations.Managers
{
    public class SpawnManager : MonoBehaviour
    {
        private void Awake()
        {
            StartLevel(1, 1);
        }

        public void StartLevel(int level, int round)
        {
            CharacterSheet.UniversalStopCsUpdateLoop = true;
            foreach (Hostile hostile in Hostile.Hostiles)
            {
                hostile.DealDamage(hostile.CurrentHp);
            }
            Hostile.Hostiles = new List<Hostile>();

            foreach (SpawnPos pos in SpawnPos.Spawns)
            {
                if (pos.level == level && pos.round == round)
                {
                    pos.Spawn();
                }
            }
            CharacterSheet.UniversalStopCsUpdateLoop = false;
        }
        
    }
}