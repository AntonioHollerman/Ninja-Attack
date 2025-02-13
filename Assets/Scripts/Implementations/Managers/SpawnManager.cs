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
        public static SpawnManager Instance { get; private set; }
        private void Awake()
        {
            Instance = this;
        }

        private IEnumerator SpawnEnemies(int level, int round)
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
            CharacterSheet.UniversalStopCsUpdateLoop = false;
        }

        public void StartLevel(int level, int round)
        {
            StartCoroutine(SpawnEnemies(level, round));
        }
    }
}