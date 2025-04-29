using UnityEngine;

namespace Implementations.Managers
{
    public class SpawnManager : MonoBehaviour
    {
        [Header("Debug Handler")]
        public bool debugModeOn;
        public SpriteRenderer sr;

        [Header("Spawner Parameters")] 
        public int instances;
        public GameObject npcPrefabs;
        
        private float MinX => transform.position.x - (transform.localScale.x / 2);
        private float MinY => transform.position.y - (transform.localScale.y / 2);
        private float MaxX => transform.position.x + (transform.localScale.x / 2);
        private float MaxY => transform.position.y + (transform.localScale.y / 2);
    }
}