using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Implementations.Extras
{
    public class LockUIPosition : MonoBehaviour
    {
        public static List<LockUIPosition> UIList = new List<LockUIPosition>();
        public Vector3 pos;
        public GameObject cam;


        private void Awake()
        {
            UIList.Add(this);
        }

        private void LateUpdate()
        {
            if (cam != null)
            {
                transform.position = new Vector3(
                    cam.transform.position.x + pos.x,
                    cam.transform.position.y + pos.y,
                    pos.z);
            }
            
        }
    }
}