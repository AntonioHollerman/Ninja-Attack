using System.Collections;
using BaseClasses;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Implementations.Managers
{
    public class SpawnManager : MonoBehaviour
    {
        private readonly Color _activeC = new Color(0.1921568f, 0.792156f, 0);
        private readonly Color _nonactiveC = new Color(1, 0, 0);
        
        [Header("Debug Handler")]
        public bool debugModeOn;
        public SpriteRenderer sr;

        [Header("Spawner Parameters")] 
        public BoxCollider c;
        public int instances;
        public GameObject[] npcPrefabs;
        public float coolDown;
        
        private float MinX => transform.position.x + c.center.x - (c.size.x / 2);
        private float MinY => transform.position.y + c.center.y - (c.size.y / 2);
        private float MaxX => transform.position.x + c.center.x + (c.size.x / 2);
        private float MaxY => transform.position.y + c.center.y + (c.size.y / 2);
        private GameObject _spawnSmokePrefab;
        
        private IEnumerator GoOnCoolDown()
        {
            c.enabled = false;
            yield return new WaitForSeconds(coolDown);
            c.enabled = true;
        }

        private IEnumerator Spawn()
        {
            float x = Random.Range(MinX, MaxX);
            float y = Random.Range(MinY, MaxY);
            GameObject prefab = npcPrefabs[Random.Range(0, npcPrefabs.Length)];
            
            Vector3 pos = new Vector3(x, y, 0);
            CharacterSheet target = Instantiate(prefab, pos, prefab.transform.rotation)
                .transform
                .GetChild(0)
                .GetComponent<CharacterSheet>();
            target.disable = true;
            
            GameObject smokeInstance = Instantiate(
                _spawnSmokePrefab, 
                target.transform.position, 
                _spawnSmokePrefab.transform.rotation);
            ParticleSystem psInstance = smokeInstance.GetComponent<ParticleSystem>();
            
            yield return new WaitUntil(() => !psInstance.isPlaying);
            target.disable = false;
            Destroy(smokeInstance.gameObject);
        }

        private void Awake()
        {
            _spawnSmokePrefab = Resources.Load<GameObject>(CharacterSheet.SpawnSmokePath);
            c = GetComponent<BoxCollider>();
            c.enabled = true;
            if (!debugModeOn && sr != null)
            {
                sr.enabled = false;
            }
        }

        private void Update()
        {
            if (!debugModeOn)
            {
                return;
            }

            sr.color = c.enabled ? _activeC : _nonactiveC;
        }

        private void OnTriggerEnter(Collider other)
        {
            Player p = other.GetComponent<Player>();
            if (p == null)
            {
                return;
            }

            for (int i = 0; i < instances; i++)
            {
                StartCoroutine(Spawn());
            }

            StartCoroutine(GoOnCoolDown());
        }
    }
}