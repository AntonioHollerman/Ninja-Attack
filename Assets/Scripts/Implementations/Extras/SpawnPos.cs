using System;
using System.Collections.Generic;
using UnityEngine;

namespace Implementations.Extras
{
    public class SpawnPos : MonoBehaviour
    {
        public int level;
        public int round;
        public GameObject prefab;
        public static List<SpawnPos> Spawns;

        private void Awake()
        {
            Spawns.Add(this);
        }
    }
}