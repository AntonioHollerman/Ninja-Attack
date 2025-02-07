using System.Collections.Generic;
using BaseClasses;
using Implementations.Extras;
using UnityEngine;

namespace Implementations.Managers
{
    public class SpawnManager : MonoBehaviour
    {
        public void StartLevel(int level, int round)
        {
            foreach (Hostile hostile in Hostile.Hostiles)
            {
                hostile.DealDamage(hostile.CurrentHp);
            }
            Hostile.Hostiles = new List<Hostile>();

            foreach (SpawnPos pos in SpawnPos.Spawns)
            {
                if (pos.level == level && pos.round == round)
                {
                    SpawnEnemy(pos);
                }
            }
        }

        private void SpawnEnemy(SpawnPos pos)
        {
            
        }
    }
}