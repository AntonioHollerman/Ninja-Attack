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
        public Collider c;
        public int instances;
        public GameObject[] npcPrefabs;
        public float coolDown;
        
        private float MinX => transform.position.x - (transform.localScale.x / 2);
        private float MinY => transform.position.y - (transform.localScale.y / 2);
        private float MaxX => transform.position.x + (transform.localScale.x / 2);
        private float MaxY => transform.position.y + (transform.localScale.y / 2);

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
            Instantiate(prefab, pos, prefab.transform.rotation);
            yield return null;
        }

        private void Awake()
        {
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