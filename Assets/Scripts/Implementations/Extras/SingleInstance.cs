using System;
using System.Collections.Generic;
using UnityEngine;

namespace Implementations.Extras
{
    public class SingleInstance : MonoBehaviour
    {
        public static List<int> ids = new List<int>();
        public int id;

        private void Awake()
        {
            if (ids.Contains(id))
            {
                Destroy(gameObject);
            }
            ids.Add(id);
        }
    }
}