using BaseClasses;
using TMPro;
using UnityEngine;

namespace Implementations.Extras
{
    public class InteractBubble : MonoBehaviour
    {
        public Player target;
        public TextMeshPro interactText;
        void LateUpdate()
        {
            transform.position = new Vector3(target.transform.parent.position.x + 0.36f, target.transform.parent.position.y + 0.22f, 0);
            transform.rotation = Quaternion.identity;
            interactText.text = target.interactCode + "";
        }
    }
}