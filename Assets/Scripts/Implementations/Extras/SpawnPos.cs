using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Implementations.Extras
{
    public class SpawnPos : MonoBehaviour
    {
        public int level;
        public int round;
        public GameObject toSpawnPrefab;
        public GameObject spawnSmokePrefab;
        public static List<SpawnPos> Spawns;

        private ParticleSystem _psInstance;
        private GameObject _smokeInstance;

        private void Awake()
        {
            Spawns.Add(this);
        }

        public void Spawn()
        {
            Instantiate(toSpawnPrefab, transform.position, spawnSmokePrefab.transform.rotation);
            _smokeInstance = Instantiate(spawnSmokePrefab, transform.position, spawnSmokePrefab.transform.rotation);
            _psInstance = _smokeInstance.GetComponent<ParticleSystem>();
            StartCoroutine(WaitToDestroySmoke());
        }

        private IEnumerator WaitToDestroySmoke()
        {
            yield return new WaitUntil((() => !_psInstance.isPlaying));
            Destroy(_smokeInstance.gameObject);
        }
    }
}