using System.Collections;
using BaseClasses;
using UnityEngine;

namespace Implementations.Extras
{
    public class Chest : MonoBehaviour
    {
        [Header("Chest Components")]
        public Sprite chestOpen;
        public Sprite chestClose;
        public GameObject[] items;

        [Header("GameObject Components")]
        public BoxCollider spawnCollider;
        public BoxCollider interactCollider;
        public SpriteRenderer sr;

        // Add an AudioClip variable to hold the sound effect
        public AudioClip openSound;

        private AudioSource audioSource; // Reference to the AudioSource component

        private float MinX => spawnCollider.transform.position.x + spawnCollider.center.x - (spawnCollider.size.x / 2);
        private float MinY => spawnCollider.transform.position.y + spawnCollider.center.y - (spawnCollider.size.y / 2);
        private float MaxX => spawnCollider.transform.position.x + spawnCollider.center.x + (spawnCollider.size.x / 2);
        private float MaxY => spawnCollider.transform.position.y + spawnCollider.center.y + (spawnCollider.size.y / 2);

        private GameObject _spawnSmokePrefab;

        private void Awake()
        {
            _spawnSmokePrefab = Resources.Load<GameObject>(CharacterSheet.SpawnSmokePath);
            sr.sprite = chestClose;

            // Get the AudioSource component
            audioSource = GetComponent<AudioSource>();
        }

        private void OnTriggerEnter(Collider other)
        {
            Player p = other.GetComponent<Player>();
            if (p != null)
            {
                p.interactIcon.SetActive(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            Player p = other.GetComponent<Player>();
            if (p != null)
            {
                p.interactIcon.SetActive(false);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            Player p = other.GetComponent<Player>();
            if (p != null && p.interacting)
            {
                p.interactIcon.SetActive(false);
                sr.sprite = chestOpen;
                p.AddExp(20);

                // Play the open sound
                if (audioSource != null && openSound != null)
                {
                    audioSource.clip = openSound;
                    audioSource.Play();
                }

                float x;
                float y;
                foreach (var item in items)
                {
                    x = Random.Range(MinX, MaxX);
                    y = Random.Range(MinY, MaxY);
                    StartCoroutine(SpawnItem(item, new Vector3(x, y, 0)));
                }

                interactCollider.enabled = false;
            }
        }

        public IEnumerator SpawnItem(GameObject item, Vector3 pos)
        {
            GameObject target = Instantiate(item, pos, item.transform.rotation);

            GameObject smokeInstance = Instantiate(
                _spawnSmokePrefab,
                target.transform.position,
                _spawnSmokePrefab.transform.rotation);
            ParticleSystem psInstance = smokeInstance.GetComponent<ParticleSystem>();

            yield return new WaitUntil(() => !psInstance.isPlaying);
            Destroy(smokeInstance.gameObject);
        }
    }
}
