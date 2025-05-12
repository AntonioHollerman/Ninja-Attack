using UnityEngine;

namespace Implementations.Extras
{
    public class SetRotation : MonoBehaviour
    {
        public GameObject target;
        void LateUpdate()
        {
            transform.position = new Vector3(target.transform.position.x + 0.25f, target.transform.position.y + 0.3f, 0);
            transform.rotation = Quaternion.identity;
        }
    }
}