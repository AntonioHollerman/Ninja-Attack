using System;
using System.Collections;
using System.Collections.Generic;
using BaseClasses;
using Implementations.Extras;
using UnityEngine;

namespace Implementations.Managers
{
    public class SpawnManager : MonoBehaviour
    {
        public float delayBetweenSpawn;
        public float lastSpawnDelay;
        public static SpawnManager Instance { get; private set; }
        private void Awake()
        {
            Instance = this;
            SpawnPos.Spawns.RemoveAll(spawn => spawn == null);
            Player.Players.RemoveAll(player => player == null);
            Hostile.Hostiles.RemoveAll(hostile => hostile == null);
            CharacterSheet.CharacterSheets.RemoveAll(sheet => sheet == null);
        }

        public IEnumerator SpawnEnemies(int level, int round)
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
                    yield return new WaitForSeconds(delayBetweenSpawn);
                }
            }

            yield return new WaitForSeconds(lastSpawnDelay);
            CharacterSheet.UniversalStopCsUpdateLoop = false;
        }
    }
}