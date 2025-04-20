using System.Collections;
using System.Collections.Generic;
using BaseClasses;
using Implementations.Characters.HostileScripts;
using UnityEngine;

namespace Implementations.Extras
{
    public class SpawnArea : MonoBehaviour
    {
        public static List<SpawnArea> Spawns = new List<SpawnArea>();
        
        public int level;
        public int round;
        public GameObject toSpawnPrefab;
        public SpriteRenderer sr;

        private Transform _hostiles;
        private Transform _players;
        private Transform _others;

        private ParticleSystem _psInstance;
        private GameObject _smokeInstance;
        private GameObject _spawnSmokePrefab;

        private void Awake()
        {
            sr.enabled = false;
            Spawns.Add(this);
            
            _hostiles = GameObject.Find("Characters/Hostiles").GetComponent<Transform>();
            _players = GameObject.Find("Characters/Players").GetComponent<Transform>();
            _others = GameObject.Find("Characters/Others").GetComponent<Transform>();
            _spawnSmokePrefab = toSpawnPrefab.GetComponent<CharacterSheet>().spawnSmokePrefab;
        }

        public void Spawn()
        {
            CharacterSheet cs = Instantiate(toSpawnPrefab, transform.position, toSpawnPrefab.transform.rotation).GetComponent<CharacterSheet>();
            SetParent(cs);
            
            _smokeInstance = Instantiate(_spawnSmokePrefab, transform.position, _spawnSmokePrefab.transform.rotation);
            _psInstance = _smokeInstance.GetComponent<ParticleSystem>();
            StartCoroutine(WaitToDestroySmoke());
        }

        private void SetParent(CharacterSheet cs)
        {
            if (cs is Player)
            {
                cs.transform.parent = _players;
            } 
            else if (cs is Hostile)
            {
                cs.transform.parent = _hostiles;
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