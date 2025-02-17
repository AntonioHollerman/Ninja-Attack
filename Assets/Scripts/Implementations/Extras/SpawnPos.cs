using System.Collections;
using System.Collections.Generic;
using BaseClasses;
using Implementations.Characters.HostileScripts;
using UnityEngine;

namespace Implementations.Extras
{
    public class SpawnPos : MonoBehaviour
    {
        public static List<SpawnPos> Spawns = new List<SpawnPos>();
        
        public int level;
        public int round;
        public GameObject toSpawnPrefab;
        public GameObject spawnSmokePrefab;
        public SpriteRenderer sr;

        private Transform _archers;
        private Transform _brawlers;
        private Transform _players;
        private Transform _others;

        private ParticleSystem _psInstance;
        private GameObject _smokeInstance;

        private void Awake()
        {
            sr.enabled = false;
            Spawns.Add(this);
            
            _archers = GameObject.Find("SpawnManager/Archers").GetComponent<Transform>();
            _brawlers = GameObject.Find("SpawnManager/Brawlers").GetComponent<Transform>();
            _players = GameObject.Find("SpawnManager/Players").GetComponent<Transform>();
            _others = GameObject.Find("SpawnManager/Others").GetComponent<Transform>();
        }

        public void Spawn()
        {
            CharacterSheet cs = Instantiate(toSpawnPrefab, transform.position, toSpawnPrefab.transform.rotation).GetComponent<CharacterSheet>();
            SetParent(cs);
            
            _smokeInstance = Instantiate(spawnSmokePrefab, transform.position, spawnSmokePrefab.transform.rotation);
            _psInstance = _smokeInstance.GetComponent<ParticleSystem>();
            StartCoroutine(WaitToDestroySmoke());
        }

        private void SetParent(CharacterSheet cs)
        {
            if (cs is Player)
            {
                cs.transform.parent = _players;
            } 
            else if (cs is Archer)
            {
                cs.transform.parent = _archers;
            }
            else if (cs is Brawler)
            {
                cs.transform.parent = _brawlers;
            }
            else
            {
                cs.transform.parent = _others;
            }
        }
        private IEnumerator WaitToDestroySmoke()
        {
            yield return new WaitUntil((() => !_psInstance.isPlaying));
            Destroy(_smokeInstance.gameObject);
        }
    }
}