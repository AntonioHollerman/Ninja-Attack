using System;
using System.Collections;
using System.Collections.Generic;
using BaseClasses;
using Implementations.Characters.HostileScripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace Implementations.Extras
{
    public class SpawnPos : MonoBehaviour
    {
        public static List<SpawnPos> Spawns;
        
        public int level;
        public int round;
        public GameObject toSpawnPrefab;
        public GameObject spawnSmokePrefab;

        public Transform archers;
        public Transform brawlers;
        public Transform players;
        public Transform others;

        private ParticleSystem _psInstance;
        private GameObject _smokeInstance;

        private void Awake()
        {
            Spawns.Add(this);
        }

        public void Spawn()
        {
            CharacterSheet cs = Instantiate(toSpawnPrefab, transform.position, spawnSmokePrefab.transform.rotation).GetComponent<CharacterSheet>();
            SetParent(cs);
            
            _smokeInstance = Instantiate(spawnSmokePrefab, transform.position, spawnSmokePrefab.transform.rotation);
            _psInstance = _smokeInstance.GetComponent<ParticleSystem>();
            StartCoroutine(WaitToDestroySmoke());
        }

        private void SetParent(CharacterSheet cs)
        {
            if (cs is Player)
            {
                cs.transform.parent = players;
            } 
            else if (cs is Archer)
            {
                cs.transform.parent = archers;
            }
            else if (cs is Brawler)
            {
                cs.transform.parent = brawlers;
            }
            else
            {
                cs.transform.parent = others;
            }
        }
        private IEnumerator WaitToDestroySmoke()
        {
            yield return new WaitUntil((() => !_psInstance.isPlaying));
            Destroy(_smokeInstance.gameObject);
        }
    }
}