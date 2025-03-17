using System;
using System.Collections;
using System.Collections.Generic;
using BaseClasses;
using Implementations.Extras;
using UnityEngine;
using System.Linq;
using Random = System.Random;

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
                hostile.DealDamage(hostile.Hp, null);
            }
            Hostile.Hostiles = new List<Hostile>();

            
            Random rnd = new Random();
            List<SpawnPos> positions = new List<SpawnPos>(SpawnPos.Spawns
                .FindAll(p => p.level == level && p.round == round)
                .OrderBy(p => rnd.Next()));
            
            foreach (var pos in positions)
            {
                pos.Spawn();
                yield return new WaitForSeconds(delayBetweenSpawn);
            }

            yield return new WaitForSeconds(lastSpawnDelay);
            CharacterSheet.UniversalStopCsUpdateLoop = false;
        }
    }
}